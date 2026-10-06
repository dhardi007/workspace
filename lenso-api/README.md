# lenso.ai — API interna descifrada

Reverse engineering del frontend (`https://lenso.ai`, bundle `common-DGc-cBrhP4.js`).

## Modelo

Vue 3 SPA + Pinia + axios. El "blur" de los resultados **no es CSS**: el backend
devuelve los resultados con `type: "LOCKED"` y campos vacíos/placeholder. El
thumbnail sí llega (vía `sc.lenso.ai/proxy/...`), pero `sources`, `urlList` real
y `resultCategory` no. Desbloquear = llamada de pago.

## Endpoints

Todos same-origin, `baseURL: "https://lenso.ai"`, sesión por **cookie** (no hay
`Authorization` header). Cookies necesarias: sesión + `facial_search_consent`.

| Método | Path | Uso |
| --- | --- | --- |
| POST | `/api/search` | búsqueda principal por imagen (todas las categorías) |
| POST | `/api/search/text` | búsqueda por texto |
| POST | `/api/search/linked` | "ver similares" de un resultado |
| POST | `/api/upload` | sube imagen base64 → devuelve `{id}` (id = segmento de la URL) |
| POST | `/api/upload/process/file` | subida multipart |
| POST | `/api/unlock-result` | **requiere plan de pago** → 401 en free |
| POST | `/api/research-mode-start` | arranca research mode |
| GET | `/api/research-mode-progress/{id}` | progreso research mode |
| POST | `/api/research-mode-results` | resultados research mode |
| GET | `/api/research-mode-filter-progress/{id}` | progreso del filtrado |
| POST | `/api/collection/{create,get,save}` | colecciones |
| POST | `/api/alert/create` | alertas |
| GET | `/api/shared-data` | perfil + free tier (`premiumStatus.limits`) |
| GET | `/api/captcha-verification/prosopo-check` | chequeo de captcha |
| POST | `/api/statistics/dmca-assist/report-generated` | stats |

## Body de `POST /api/search`

```json
{
  "image":   { "id": "<segmento de /en/results/...>" },
  "effects": { "rotation": 0 },
  "selection": { "top": 0, "left": 0, "right": 1, "bottom": 1 },
  "domain": "", "text": "",
  "page": 1,
  "type": "duplicates",
  "sort": "",
  "seed": 0,
  "facial_search_consent": 0
}
```

Gotchas:

- `seed` debe ser **número** (`0`). Mandarlo como `""` → `400 Not able to
  deserialize data provided.` (error genérico, no dice qué campo).
- `facial_search_consent` va como `1`/`0` en search, pero como **boolean** en
  `/api/upload`.
- `selection` son coordenadas normalizadas 0..1. La URL los reparte en
  `?t=&l=&w=&h=` y el frontend los recombina: `right = w + l`,
  `bottom = t + h`.
- `type` acepta: `people`, `duplicates`, `places`, `related`, `similar`, `""`.
- Un body mal formado da `500 An unexpected error occurred.` (no 400). Pasa un
  id de imagen inexistente y eso es lo que obtienes.

## Respuesta

```
results[]  { hash, distance, proxyUrl, urlList[], width, height, type,
             affiliateUrl, category, tileType, debug }
urlList[]  { imageUrl, sourceUrl, title, lang, md5Input }
top-level  { results, content, debug, availablePages, detectedFaces,
             category, searchHash, landmark, researchMode }
```

`type: "LOCKED"` → resultado bloqueado. `tileType` ∈
`SEARCH_RESULT_TILE | SEE_MORE_TILE | ALERT_TILE`.

`POST /api/unlock-result` → `{proxyUrl, hash, category}` (ojo: el campo
`category` del request es `resultCategory` en el modelo del cliente).

## El token y su "cifrado"

`window.lenso.tkn` (352 chars, base64) se manda **doble** en el body de
upload: `token` (raw) + `tokenEncrypted`. `Il()` en el bundle:

```js
Il = e => {
  const n = new TextEncoder().encode(e).map(a => a ^ 170);   // XOR 0xAA
  for (let a = 0; a < n.length; a++) n[a] = ~n[a] ^ a;       // NOT ^ índice
  let s = String.fromCharCode(...n);
  s = s.slice(2, s.length - 2);                              // quita 2 edges
  const r = MD5(s).toString();
  let o = [];
  for (let a = 0; a < r.length; a += 4)
    a === 0 || a === 8 || a === 16 || a === 24 || o.push(r.slice(a, a + 4));
  return o.toReversed().join("");                             // 16 hex
};
```

Detalle: el MD5 son 32 hex = 8 chunks de 4. El loop **descarta** los chunks en
offset 0, 8, 16, 24 → se quedan los de offset 4, 12, 20, 28. Luego
`.toReversed()` invierte el orden. Resultado: 16 hex chars.

Es **ofuscación anti-bot, no criptografía**: sin `tkn` no hay request, pero el
`tokenEncrypted` no protege nada que `token` no exponga. Sirve para que un
scraper que solo copie el segundo campo falle.

## Free tier

```json
"premiumStatus": {
  "hasExportEnabled": false, "hasUnlimitedUnlocks": false,
  "limits": { "api": 0, "alerts": 3, "unlocks": 0 },
  "usage": { "api": 0, "alerts": 0, "activeAlerts": 0, "unlocks": 0 },
  "type": "free"
}
```

- `unlocks: 0` → `POST /api/unlock-result` responde **401**.
- `alerts: 3` → 3 alertas gratis.
- `api: 0` → sin API oficial en free.
- `freeSearchesPerformed` / `freeSearchLimit` = 5 en el HTML del SSR.

Los thumbnails de resultados bloqueados **sí** son accesibles
(`sc.lenso.ai/proxy/<hex>`), y las imágenes originales en `urlList[].imageUrl`
también. Lo que no llega es `sourceUrl` real y el conteo de fuentes.

## Uso

```bash
python3 lenso_api.py <image_id> duplicates,related,similar
```

```python
from lenso_api import Lenso
c = Lenso()
d = c.search(image_id, 'duplicates')
for r in d['results']:
    print(r['type'], r['hash'], r['proxyUrl'])
```

## Notas de anti-abuso

Esto es una API no documentada de un producto de pago. El código sirve para
automatizar tu propia cuenta y para proyectos de investigación. Notas:

- El 401 en `unlock-result` es un muro de pago deliberado, no un bug. No lo
  intentes evadir; requiere un plan de pago real.
- `unlocks`, `alerts` y `usage` se trackean server-side por cookie de sesión.
  Los thumbnails vía `sc.lenso.ai/proxy/` no requieren sesión.
- Respeta los 5 searches gratis del free tier y pon rate limit si lo usas en
  loop: los 5XXXXxis el server quien manda.