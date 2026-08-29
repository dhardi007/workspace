# PLAN DE ENTRENAMIENTO — Entrevista IntelliSys (2026-08-26)

Sesión `vim-learn` · Profesor de Diego · Basado en la entrevista con Amaya Almonte (PM) y Carlos Cruz (Profesor Developer Senior)

---

## Diagnóstico general

Carlos hizo 7 pruebas técnicas. El patrón de los fallos 🚫 NO fue falta de habilidad,
fue **no saber reconocer/nombrar la información que ya estaba en el propio repo de Diego**.

Resumen de resultados:

| # | Prueba | Resultado | Tiempo |
|---|--------|-----------|--------|
| 1 | Cambiar color CTA a rojo (Home.tsx) | ✅ | 3m |
| 2 | Quitar background carrusel (Home.tsx) | ✅ | 10m |
| 3 | ¿Qué es React Redux? (AuthContext.tsx) | 🚫 | — |
| 4 | Hacer contador en React (App.tsx + contador.ts) | 🚫 | — |
| 5 | Crear ruta en back para enviar un número | 🚫 | — |
| 6 | Explicar New Set | ✅ (con ayuda) | — |
| 7 | email.ts (backend util) | ✅ (con corrección) | — |

**Lección central de todas: conocer tu propio proyecto y saber explicarlo.**

---

## Fallo 1 — "No supe responder qué es React Redux"

### Qué pasó
Carlos preguntó por React Redux. Diego no pudo responder, aunque **su proyecto lo usa de punta a punta**:
- `src/store/store.ts` → `configureStore` con 17 reducers
- `src/store/slices/authSlice.ts` → reducer de auth
- `src/contexts/AuthContext.tsx` → `useAppDispatch()` / `useAppSelector()` sincronizando Redux ↔ Context

### Qué faltaba (concepto)
- **NOMBRAR** lo que el código ya hace. Diego lo usa pero no tenía el vocabulario para explicarlo.

### Objetivo de aprendizaje
Que Diego pueda explicar, con sus palabras y apuntando a su código:
1. ¿Qué es Redux y qué problema resuelve? (estado global compartido entre componentes sin prop-drilling)
2. ¿Qué es Redux Toolkit? (la forma moderna, `configureStore`, `createSlice`) — es lo que usa su repo
3. ¿Qué es un `slice`? (`authSlice` = estado + reducers de auth juntos)
4. `useSelector` (leer) vs `useDispatch` (escribir)
5. ¿Por qué en `AuthContext` sincroniza Redux con Context? (Context para acceder fácil al usuario, Redux como store global)

### Métodos
- **Elaboración** (conectar con su código real)
- **Feynman** (que explique al final con sus palabras)

---

## Fallo 2 — Contador en React (App.tsx / contador.ts)

### Qué pasó
- Diego creó `src/util/contador.ts` que es un **IIFE con closure** (patrón módulo):
  ```ts
  const contador = (function () {
    let count = 0
    return function () {
      count++
      return count
    }
  })()
  export default contador
  ```
- Lo importó en `App.tsx` (`import contador from "./util/contador"`, línea 34)
- **Pero**: no hizo el botón/componente que lo renderizara. Carlos pidió "un contador en React" = un componente con UI, y Diego dio una función lógica sin UI.

### Qué faltaba
- **Distinguir lógica vs UI**. Un IIFE es lógica pura; un contador en React necesita un componente con `useState` y un `<button>`.

### Objetivo de aprendizaje
1. ¿Qué es una **IIFE**? `(function(){...})()` se ejecuta al instante.
2. ¿Qué es un **closure**? La función interna "recuerda" `count` aunque el IIFE ya retornó.
3. ¿Por qué el IIFE no es un componente React? Porque un componente devuelve JSX y vive en el ciclo de render de React.
4. El patrón correcto: **`useState`** para el valor + `<button onClick>` para incrementar.

### Métodos
- **Analogía** (closure = mochila que guarda `count`)
- **Mini-ejercicio** (hacer el componente contador)

---

## Fallo 3 — Ruta en el backend para "enviar un número" (front → Postgres)

### Qué pasó
- Carlos pidió crear una ruta para enviar un número (desde el DOM/front al backend/Postgres).
- Diego se confundió sobre si el proyecto era **NestJS** o Express → perdió el hilo y se rindió.

### Confirmación técnica (verificada en el repo)
**Es Express + Node, NO NestJS.**
- `src/app.ts:3`: `import express, { json } from "express"`
- Estructura: `routes/` (express.Router) + `controllers/` + `Services/`
- Reconocimiento rápido de NestJS: `@nestjs/common`, decoradores `@Controller()/@Get()/@Module()`, `NestFactory.create()`. Diego NO tiene nada de eso.

### Cómo se montan las rutas en SU proyecto (patrón real)
1. `routes/xx.routes.ts` → crea `const router = express.Router()` y define `router.get("/...", handler)`
2. `index.routes.ts` → `router.use('/api', xxRouter)`
3. `app.ts` → `app.use(indexRoutes)` (línea 148)

### Qué faltaba (concepto)
- El **flujo completo front → back**: el front hace `fetch/axios` a `VITE_BE_BASE_URL/api/...`, el back lo recibe en el router → controller → service → DB.

### Objetivo de aprendizaje
1. Armar una ruta GET/POST en Express en su repo (usando su patrón).
2. Que el front la consuma con axios (él ya usa axios en AuthContext/notify.ts).
3. Persistir en Postgres vía TypeORM (ya configurado en `config/database.ts`).
4. Darse cuenta del cue: `numeroContador.ts` en controllers está **vacío (0 bytes)** — ahí iba su intento.

### Métodos
- **Aprender haciendo** (crear la ruta de verdad)
- **Debugging guiado** (que aísle el error si algo no anda)

---

## Fallo 4 — Vim motions lentos (perdió ~10 min buscando un bloque)

### Qué pasó
- Quitar el carrusel le costó 10m porque no dominaba la búsqueda eficiente. Su hermano le dio tips:
  - `Space + /` → live grep en lazyvim (buscar en todos los archivos)
  - `Space + s + w` → buscar la palabra bajo el cursor
- Diego NO usó vim en la grabación, se movió a lo "bruto".

### Objetivo de aprendizaje (refresco de memoria + práctica)
- **En terminal/cli**: `rg "palabra"` (ripgrep) o `grep -rn "palabra" src/`
- **En lazyvim (Neovim)**:
  - `Space + /` = Live Grep (búsqueda global, tipo ripgrep dentro de nvim)
  - `Space + s + w` = símbolo bajo el cursor / buscar la palabra actual
  - `Space + f + f` = buscar archivo por nombre
  - `*` = buscar palabra bajo cursor en el archivo
- Sentar la regla: **siempre usar búsqueda, nunca el ojo/scroll manual**.

### Métodos
- **Repetición espaciada** (reforzar cada vez que toquemos un archivo)
- **Mini-ejercicio** (buscar un token específico en el repo con ripgrep)

---

## Fallo 5 (bonus) — Bug en el comentario de New Set (contador.ts)

### Qué pasó
- Diego pegó como "check" el ejemplo de New Set, pero la versión que dejó en `contador.ts:11-16` tiene errores:
  ```ts
  const valores = ["a", "b", "c", "d", "e", "e", "cd"]
  console.log(valores.length) // 6   ← MAL: el array tiene 7 elementos (a,b,c,d,e,e,cd)
  const valores2 = new Set(["a", "b", "c", "d", "e", "e", "cd"])
  console.log(valores2.length) // 5   ← MAL: los Set NO tienen .length, usan .size; y el tamaño sería 6
  ```

### Objetivo de aprendizaje
1. `Array.length` → cuenta TODOS los elementos (incluidos duplicados).
2. `Set` elimina duplicados automáticamente y se mide con **`.size`**, NO `.length`.
3. Corregir el ejemplo en el archivo como ejercicio.

### Métodos
- **Mini-ejercicio** (corregir el comentario en su repo)

---

## Fallo 6 — Inglés (fuera de alcance ACÁ)

Diego lo reconoció: le fue mal en inglés. Ya lo está atacando con **Duolingo**. No entra en el plan técnico de esta sesión (es un tema de práctica diaria, no de código).

---

## Orden sugerido de trabajo (de mayor a menor impacto)

1. **Redux** (fallo #1) — era 100% de su propio código; el mayor "easy win"
2. **Express ruta + conectar front** (fallo #3) — despeja la confusión NestJS/Express
3. **Contador React: IIFE vs componente** (fallo #2) — cierra la brecha lógica/UI
4. **New Set** (fallo #5) — corrección rápida del bug en contador.ts
5. **Vim motions** (fallo #4) — transversal, se refuerza durante TODAS las tareas

---

## Formato de cada lección (Profesor)

Por cada tema, en el chat:
1. **Razonamiento** → por qué importa y el concepto real.
2. **Snippet de referencia** en el chat (nunca escribo archivos del proyecto).
3. **Mini-ejercicio** → Diego lo aplica/corrige en su repo.
4. **Verificación** → build / comando / explicación de Diego.
5. **Feynman** → Diego explica el concepto con sus palabras.

---

## SECCIÓN EXTRA — Charla con Carlos (sábado 2026-08-29)

Diego asistió a una charla del mismo Senior Carlos (el de la entrevista). Temas nuevos
a estudiar, conectados con los conceptos ya vistos:

| Tema | Qué es | Conexión con la entrevista |
|------|--------|------------------------------|
| **Frameworks de React** | Usar un framework ≠ instalar una lib. Configurarlo manualmente es decisión de cada quien. Ej: Auth.js (`authjs.dev`) para Next/React, Svelte, Express. | Fallo #1 (Redux): su auth custom en AuthContext es lo que Auth.js abstrae |
| **Mermaid (mermaid.ai)** | Diagramar workflows (flowcharts/sequence/ERD) como texto. | Fallo #3 (ruta back): el flujo front→back se explica perfecto con un sequence diagram |
| **Helix vs Neovim/VSCode** | Helix es modal (como vim) pero menos configurable; Neovim/LazyVim más potente. **No es mejor que el vim actual de Diego.** | Fallo #4 (vim motions): su stack vim sigue siendo válido |
| **Jest** | Framework de tests automatizados recomendado por Carlos. | Fallo #2 (contador): con tests se habría detectado antes la falta de UI |
| **pnpm vs npm** | pnpm es más eficiente que npm; son compatibles. Diego terminó con npm — no grave. | Transversal (builds de sus repos) |
| **TypeScript config** | `tsconfig.json` ya existe en sus repos (`strict`). | Transversal |
| **`isResponse` / `Machine` / `extend`** | ⚠️ PENDIENTE: Diego cree que es backend. Confirmar qué eran antes de documentar. | ¿Backend? ¿State machine? ¿TypeScript? |

---

## Nota final

Regla Profesor confirmada: **NO escribo código en archivos**. Los ejercicios los hace Diego.
Los snippets van SOLO en el chat como referencia.
