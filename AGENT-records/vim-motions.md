# Vim motions — chuleta rápida (transversal)

Se usa en TODAS las sesiones. Diego quiere que
SIEMPRE le recuerde la clave vim del momento.

## En terminal (CLI)

| Comando              | Qué hace                            |
| -------------------- | ----------------------------------- |
| `rg "palabra"`       | buscar en archivos (ripgrep)        |
| `rg "palabra" src/`  | buscar acotado a una carpeta        |
| `grep -rn "p" src/`  | alternativa clásica                 |

## En Neovim / LazyVim

| Key                | Qué hace                                |
| ------------------ | --------------------------------------- |
| `Space + /`        | **Live Grep** (búsqueda global)         |
| `Space + s + w`    | buscar la palabra bajo el cursor        |
| `Space + f + f`    | buscar archivo por nombre               |
| `*`                | buscar palabra bajo cursor en archivo   |
| `/`                | buscar en el archivo actual             |
| `n` / `N`          | siguiente / anterior coincidencia       |
| `Space + g + g`    | ir al inicio del archivo                |
| `viw`              | **seleccionar** la palabra (visual)     |
| `Space + s + r`    | Snacks **Rename** (reemplazo)           |

## ⚠️ `*` NO selecciona la palabra

**Confusión frecuente de Diego:** en Vim/Neovim,
`*` puesto sobre una palabra la **busca** (la resalta
en todo el archivo), pero **NO la deja "seleccionada"**
como un objeto que un reemplazo pueda tomar.
Por eso, al pulsar luego `Space + s + r`, el campo
Search/Replace del selector aparece **vacío**.

### Cómo se comporta cada editor

| Acción                | Helix                           | Neovim (Vim)                    |
| --------------------- | ------------------------------- | ------------------------------- |
| Poner cursor          | El cursor es la selección       | El cursor es un punto           |
| "Marcar" la palabra   | La selección es objeto nativo   | `*` = busca, NO selecciona      |
| Ejecutar reemplazo    | Afecta la selección directo     | Necesitás seleccionar primero   |

**Conclusión:** en este punto Helix es más directo
(selección nativa). Lo reconozco. La ventaja de
Neovim es que esa "selección explícita" te da
control total cuando la dominás.

### Cómo seleccionar la palabra en Neovim

- `viw` (o `viW`) → **selecciona** la palabra bajo
  el cursor (visual). `v`=visual, `iw`=inner word.
- Luego ya podés: `d` (borrar), `c` (cambiar),
  `y` (copiar), `r letra` (reemplazar), etc.
- Para renombrar toda la palabra en el archivo:
  con `viw`, luego el flujo de rename.

### Flujo correcto para "renombrar una interface"

```vim
viw            " selecciona la palabra (visual)
y              " (opcional) copiarla
:grep "authUser" %  " o usar Space+/ para buscar
```

Y para reemplazo controlado (ver sección de
reemplazo):

```vim
:%s/PalabraVieja/PalabraNueva/g
```

### Resumen anti-confusión

> `*` = **buscar** (resalta matches).
> Para **seleccionar** la palabra usá `viw`.
> Son conceptos distintos. En Helix la selección
> es nativa; en Neovim se hace con `viw`.

## Grug-Far / Snacks Rename (`Space + s + r`)

`Space + s + r` abre el selector con los campos
y, dentro, este HUD de acciones.

### Campos (navegás con Tab / Shift+Tab)

```text
Search → Replace → Files Filter → Flags → Paths
```

### Acciones del HUD

| Tecla     | Acción      | Qué hace                                  |
| --------- | ----------- | ----------------------------------------- |
| `\r`      | **Replace** | Aplica el reemplazo en el buffer          |
| `g?`      | **Help**    | Abre la ventana de ayuda del HUD          |
| `\s`      | Sync All    | Guarda TODAS las líneas editadas          |
| `\l`      | Sync Line   | Guarda solo la línea actual               |
| `\n`      | Sync Next   | Guarda la línea actual y pasa a siguiente |
| `\p`      | Sync Prev   | Guarda la línea actual y pasa a anterior  |
| `\v`      | Sync File   | Guarda todos los cambios del archivo      |
| `\j`      | Apply Next  | Aplica línea actual, va a siguiente       |
| `\k`      | Apply Prev  | Aplica línea actual, va a anterior        |
| `<Tab>`   | Next Input  | Salta al siguiente campo (cicla)          |
| `<S-Tab>` | Prev Input  | Salta al campo anterior                   |

### Flujo completo de reemplazo

Para reemplazar **todos** de una pasada:

```text
1. Space + s + r
2. Escribís Search + Replace
3. \r        ← APLICA el cambio en el buffer
4. \s        ← Sync All: guarda en los archivos
```

Para reemplazar **uno por uno**:

```text
1. Space + s + r
2. Search + Replace
3. \r         ← modifica las líneas del buffer
4. borrá con `dd` las líneas que NO querés
5. \l  o  \n  o  \v   ← sync solo lo que dejaste
```

> `\j` / `\k` (Apply Next/Prev) = aplicar caso
> por caso avanzando.
>
> ⚠️ **Antes de usar Grug-Far, asegurate de que
> Paths apunte a un solo proyecto** (o haz
> `:lcd`/`:cd` primero). Si no, ripgrep barre
> todo `~/` y tocás repos que no querés.

## Combo mágico: reemplazo selectivo

Para reemplazar **eligiendo cada caso** (sin barrer
todo el archivo, sin riesgo de `:cfdo`):

```text
1. cursor sobre la palabra
2. *            → busca la palabra
3. cgn          → cambia el próximo match
4. {texto}      → escribís el reemplazo
5. <Esc>
6. .            → repite el cambio en siguiente
7. n            → (opcional) salteás uno
8. .            → seguís reemplazando
```

Ventajas: elegís cada uno, no borra de más,
no toca archivos que no querés.

## Truco del `*` + `:%s//`

```vim
cursor sobre la palabra → * → :%s//reemplazo/g
// vacío = usa la palabra que marcaste con *
```

`*` busca la palabra bajo el cursor;
`:%s//texto/g` reemplaza con `//` vacío
aprovechando ese patrón de búsqueda.

## Regla de oro

> **Siempre buscar con `rg` / `Space+/`,
> nunca escanear con el ojo.**
> Esto evitó 10 min perdidos en la entrevista.

## Cómo se busca "Background"

```bash
rg -n "Background" workspace/ptd-talento-front/src
```

```markdown
# Luego Space + / en nvim, escribir Background,
# Enter, y navegar con n/N
```
