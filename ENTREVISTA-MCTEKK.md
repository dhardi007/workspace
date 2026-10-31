# Preparación entrevista técnica — MCTEKK

**Fecha:** viernes 9 de octubre de 2026, 10:30–11:00 (AST)
**Quién:** Jennifer Herasme, `jenn@mctekk.com` — cofundadora y CEO de MCTEKK
**Formato:** 30 min, virtual (enlace por correo de Cal.com)
**Idioma:** español, con posible salto a inglés para el bloque técnico
**Proyecto:** `dhardi-laravel` (Laravel 13 + Inertia/React + TypeScript)

---

## 0. Revisa esto ANTES de la llamada

### 0.1 El enlace de tu nota no es el que describes

En tu nota de postulación aparece:

> `https://portfolio-terminal-dhardi.vercel.app/ A Laravel 13 + Inertia/React
> + TypeScript application I built and shipped in the last few days...`

Ese enlace es el **Portfolio Terminal, hecho en SvelteKit**. No es el proyecto
Laravel que describe el texto y que es de lo que se van a hablar. Verifica qué
URL llegó en el correo. Si llegó mal, reenvíala corregida hoy mismo, antes del
viernes:

- ✅ `https://dhardi-laravel.vercel.app/` ← el proyecto Laravel
- ✅ `https://github.com/dizzi1222/dhardi-laravel` ← el código
- ❌ `portfolio-terminal-dhardi.vercel.app` ← SvelteKit, no Laravel

### 0.2 Verifica que `dhardi-laravel.vercel.app` está en verde

```bash
for u in "/" "/?lang=es" "/up"; do
  printf "%-10s " "$u"; curl -sS -o /dev/null -w "%{http_code}\n" "https://dhardi-laravel.vercel.app$u"
done
```

---

## 1. Contexto de la empresa

| | |
|---|---|
| Empresa | MCTEKK S.R.L. — Santo Domingo, República Dominicana (Ensanche Naco) |
| Producto | **Kanvas**: backend API-driven para unificar inventarios, pagos, usuarios, productos y workflows |
| Dirección | Agentic Commerce — automatización con IA sobre operaciones de comercio empresarial |
| Modalidad | **100 % remoto** (según sus vacantes) |
| Tu contacto | Directo con la CEO, no con un reclutador externo |

**Conexión con tu proyecto:** ellos resuelven el mismo tipo de problema —
infraestructura fragmentada que hay que unificar. Tu app demuestra dos cosas que
ellos tocan a diario: resolver datos por cliente y mantenerse disponible cuando
un proveedor externo (una API de IA) falla.

---

## 2. Primeros 60 segundos

> *"Hola Jennifer, gracias por el tiempo. Soy Diego, full-stack con base en
> Node y React. Lo que les envié no es un portafolio más — lo construí en una
> semana sobre el stack que ustedes usan: Laravel 13 con Inertia y React,
> tipado estricto, suite de PHPUnit verde y en producción en Docker. Elegí dos
> problemas que ya sabían que tenían: multi-tenancy y fiabilidad sobre modelos
> de lenguaje. Les muestro el código cuando quieran."*

**No** empieces por mencionar que trabajas en la RD, ni por pedir perdón por no
tener tres años de Laravel. Empieza por el artefacto. Es lo que les llamó la
atención.

---

## 3. Preguntas técnicas — lo que van a preguntar, con la respuesta real

### 3.1 Multi-tenancy

**Preguntarán:** *"¿Lo resuelves por subdominio, por cabecera o por base de
datos separada?"*

**Respuesta honesta — ninguna de las tres clásicas:**

Mi modelo es **una instancia por cliente**, el mismo que describen en su
infraestructura. El tenant se resuelve desde `TENANT_SLUG` (variable de entorno
quemada en el despliegue), con dos override: sesión y query string `?tenant=`.

```php
// app/Support/TenantContext.php
session → query string → env TENANT_SLUG
```

**La parte fuerte:** el aislamiento no depende de que un controlador se acuerde
de filtrar, porque está en un global scope:

```php
// app/Models/Concerns/BelongsToTenant.php
static::addGlobalScope('tenant', fn ($query) =>
    $query->where('tenant_id', app(TenantContext::class)->id())
);
```

Y una fila se **niega a crearse sin tenant**, no se crea vacía:

```php
static::creating(function ($model) {
    abort_if($model->getAttribute('tenant_id') === null, 500, ...);
});
```

**La pregunta clásica:** *"¿Cómo garantizas que el Tenant A no vea datos del
Tenant B?"* → No es una comprobación en cada endpoint: es que **toda consulta
pasa por el scope antes de llegar a la base**. Si olvidas escribir el `where`,
el scope lo pone. Si lo escribes mal, te da 500, no datos cruzados.

**Punto a señalar:** `php artisan tenant:provision acme --name="Acme AG"`
crea la instancia, migra, seedea e imprime las variables de entorno que hay que
setear. Levantar un cliente es un comando, no un día de trabajo.

📄 **Muéstrales:** `app/Models/Concerns/BelongsToTenant.php`,
`tests/Feature/TenantResolutionTest.php` (6 tests: resolución desde entorno,
override por query, tenant inexistente → `null`, y **rechazo de escritura sin
tenant**).

⚠️ **No lo exageres:** es multi-tenancy por instancia, no un esquema compartido
con seguridad a nivel de fila. Si preguntan por *schema-per-tenant* o *row-level
security*, di que esta es la decisión para su caso (un cliente por despliegue)
y que conoce la alternativa, no que la has implementado en producción.

---

### 3.2 Circuit breaker

**Preguntarán:** *"¿Qué librería usas? ¿Qué umbrales tienes? ¿Cómo funcionan los
estados?"*

**Respuesta:** está **escrito a mano**, no usa librería.

| Estado | Cuándo entra |
|---|---|
| `closed` | Por defecto. Todo pasa. |
| `open` | `LLM_BREAKER_THRESHOLD` (5) fallos seguidos → se rechazan intentos durante `LLM_BREAKER_COOLDOWN` (30 s) |
| `half_open` | Pasó el cooldown → se admite **exactamente una** sonda |

El truco de `half_open` es la una-sonda:

```php
// app/Llm/Reliability/CircuitBreaker.php
return $this->cache->add($this->probeKey(), true, $this->cooldownSeconds);
```

`Cache::add()` solo devuelve `true` si la clave **no existía**. El primer que
llega tras el cooldown crea la clave y pasa; los concurrentes ven la clave
creada y no pasan. Un backend que se está recuperando no recibe un flood.

**Preguntarán también:** *"¿Qué errores activan el breaker?"* → Los
`retryable`: 429 y 5xx sí. **Un 400 nunca**, porque es un error del que pide, y
reintentarlo solo consume cuota. Y una **violación de guardrail nunca se
reintenta** — rechazar es el resultado correcto, no un fallo.

📄 **Muéstrales:** `app/Llm/Reliability/CircuitBreaker.php`,
`tests/Unit/CircuitBreakerTest.php` (8 tests: 5 fallos abren, el éxito lo
cierra, y **la sonda del half-open se verifica de forma concurrente**).

---

### 3.3 Política de reintentos

```php
// app/Llm/Reliability/RetryPolicy.php
base_delay_ms   = 250
multiplier      = 2.0      // 250 → 500 → 1000 → 2000
max_delay_ms    = 4000
max_attempts    = 3
```

Dos decisiones que conviene tener claras:

1. **`Retry-After` tiene prioridad** sobre la curva local. Si el proveedor dice
   "vuelve en 15 s", su dato es mejor que mi heurística — pero se le aplica el
   tope de 4000 ms.
2. **El jitter es inyectable.** No es un detalle: sin eso los reintentos se
   producen sincronizados entre instancias (el *thundering herd*), y como además
   no puedo dejar que el test se duerma, el `Sleeper` también es una interfaz.

📄 `tests/Unit/RetryPolicyTest.php` — 11 tests, incluido el límite de jitter y
que `Retry-After` lo desplaza.

---

### 3.4 Prompt caching

La clave no es la pregunta, es **todo el prompt**:

```
llm:cache:t{tenant}:{provider}:{model}:sha256(json_encode(messages+system+maxTokens+temperature))
```

Si se cambia el system prompt o la temperatura, cambia la clave. Si cambia el
tenant, cambia la clave — un cliente no puede recibir la respuesta cacheada de
otro.

📄 `tests/Unit/PromptCacheTest.php` — acierto, fallo, aislamiento por tenant, y
que cada parámetro que afecta la respuesta cambia la clave.

---

### 3.5 Calidad y tests

**90 tests, 357 aserciones, PHPUnit 12.** No Pest — no se puede instalar en
Laravel 13 (su plugin declara soporte solo para 11 y 12). Si preguntan por eso,
es una respuesta técnica concreta, no una evasión.

| Fichero | Qué protege |
|---|---|
| `RetryPolicyTest` | backoff, límites, jitter, `Retry-After` |
| `CircuitBreakerTest` | apertura, cierre, la sonda de half-open |
| `PromptCacheTest` | aislamiento entre clientes |
| `GuardrailsTest` | reglas de entrada y salida, caso insensible |
| `TenantResolutionTest` | resolución y **rechazo de escritura sin tenant** |
| `AssistantEndpointTest` | streaming SSE, guardrail devuelve 422 |
| `TranslationCoverageTest` | que falte una traducción rompe el build |
| `GeneratedUrlsTest` | que las URLs salgan en https |

**La pregunta que prepara esto:** *"¿Cómo simulas una respuesta fallida del LLM
en tus tests?"* → No mockeas la red: inyecto dependencias. `RetryPolicy` recibe
un `Sleeper` y una closure de jitter; el test registra las esperas en un array y
las aserta sin dormir. El circuit breaker usa el store de caché, que en tests es
`array` (`phpunit.xml` → `LLM_CACHE_STORE=array`), así que cada test arranca en
estado limpio.

📄 `tests/Feature/CvNoticeTest.php` tiene un test con `DB::listen()` que **lanza
una excepción en cada INSERT** — así verifico que el endpoint sigue respondiendo
cuando la base no acepta escrituras. Es exactamente el caso de una instancia sin
disco.

---

### 3.6 El golden dataset

```bash
php artisan evals:run     # 12 casos: de, es, en
```

Repite los casos contra la cadena viva, guarda el resultado en
`llm_eval_results` y **sale con código distinto de cero si un caso falla**. Por
eso está en el CI: un cambio de prompt que rompe las respuestas esperadas no se
descubre en una conversación con el cliente, se descubre en el build.

---

### 3.7 Stack en una frase

Laravel 13.34 · PHP 8.4 · Inertia 3.8 · React 19.3 · TypeScript **strict**
(`noUncheckedIndexedAccess`) · Tailwind 4 · PHPUnit 12 · Docker (FrankenPHP +
Caddy) · Vercel container · GitHub Actions (Pint, PHPUnit, tsc, build, evals).

---

## 4. Si preguntan por tu experiencia

**No empiezes por La Sirena.** Tu trayectoria técnica real, en este orden:

1. **Lead Tech en PTD-Talento** (CIC) — equipo de 4, 18 sprints, **74 PRs
   e issues**. Corrección de un P1 de autorización en una ruta de favoritos,
   migraciones idempotentes, audit trail, rate-limiting anti-spam, subidas a
   GCS, y scoring de perfil con LLM sobre OpenRouter con fallback heurístico.
2. **Freelance y propios** — del primer commit al deployment, con CI y
   operación.
3. **Bootcamp y técnicatura** — CIC y Liceo Técnico Luis Ernesto Gómez Uribe.
4. **La Sirena (5 meses)** — solo si preguntan por trabajo formal. Menciona
   aprendizaje, no el CV completo.

---

## 5. Preguntas para que tú hagas (5 min al final)

1. *"¿Cómo mide Kanvas el éxito de un cliente nuevo?"* — pregunta de negocio,
   no de código. Diferenciarte de candidatos que preguntan por el stack.
2. *"¿El equipo es solo frontend/backend o full-stack?"*
3. *"¿Cómo se despliegan hoy — hay pipeline de CI o manual?"*
4. *"¿Kanvas multi-tenancy ya está resuelto o es parte de lo que está abierto?"*
   — directamente útil: si está abierto, tu proyecto es la respuesta.
5. *"¿Qué hay que demostrar en los próximos 30 días para que la conversación
   sobre esta vacante sea razonable?"*

---

## 6. Lo que NO debo afirmar

| Tema | Por qué |
|---|---|
| Laravel "en producción" años | Es un proyecto de una semana, y ya lo dije en el correo |
| Multi-tenancy con esquema compartido y seguridad por fila | Mi modelo es instancia por cliente |
| `.NET`, `C#`, `SQL Server`, `Angular` sin código | **Cero archivos .cs en todo el workspace.** Si preguntan, di que están en el CV como formación pero el código verificable es Node/React/PHP |
| Telemetría en producción | La base está horneada en la imagen: el panel mostrará ceros. Dilo si preguntan por los números en vivo |
| Años de experiencia | Eres técnico en formación con un proyecto Lead Tech. Di la duración concreta |

**La respuesta honesta sobre el stack:** *"Mi producción fuerte es Node con
Express, TypeORM y PostgreSQL, y React con TypeScript. Laravel es el stack sobre
el que construí este proyecto en una semana porque es el que piden, y tengo la
aprendizaje visible con 90 tests, CI y deploy. Lo que no voy a hacer es
afirmar que llevo años en Laravel."*

---

## 7. Salto a inglés (frases clave)

Si dicen *"let's switch to English"*:

> "Let me walk you through the tenant resolution. It's set per deployment
> through an environment variable, and the isolation is enforced at the query
> layer rather than in each controller — there's a global scope on every model
> that carries a tenant ID, and creating a row without a tenant fails instead of
> silently writing a null."

> "The circuit breaker is hand-written, not a library. Five consecutive failures
> open it for thirty seconds. After that, exactly one probe is admitted —
> concurrent requests are refused — because `Cache::add` only succeeds if the
> key didn't exist."

> "Retry policy: exponential backoff from 250 milliseconds, capped at four
> seconds, with jitter. A `Retry-After` header overrides the curve, clamped to
> the cap. A 400 is never retried, and a guardrail rejection is never retried
> either — refusing is the correct outcome, not a failure."

---

## 8. Logística

- [ ] Confirmar que la URL del proyecto llegó correcta en el correo (§0.1)
- [ ] Reabrir el sitio en el navegador y comprobar que carga sin avisos
- [ ] Tener abierto el repo en GitHub con `tests/` visible
- [ ] Revisar el CV: si aparece .NET/C# sin código, decidir si lo dices tú antes
- [ ] Test de audio 10 min antes
- [ ] Tener a mano: `php artisan test` en terminal para un `--filter` en vivo
