# Sesión Redux Toolkit — razonamiento de Diego (aprox 50-60 min)

Fecha: 2026-08-30 · Sesión `vim-learn` · Método: aprender haciendo + elaboración + Feynman

Referencia real: `workspace/ptd-talento-front/src/store/slices/*` + `store/store.ts` + `contexts/AuthContext.tsx` + `hooks/`

---

## Lo que Diego entendió (correcto)

- **Slice** = `initialState` + `reducers` agrupados. Son "incrementales" (acumulan estado).
- **`name`** del slice = clave del estado dentro del store (ej. `feedBack`).
- **Una acción = una función reducer** que cambia el estado.
- **`useAppSelector` LEE** un pedazo del store; **`useAppDispatch` ESCRIBE** (dispara acciones).
- **`PayloadAction`** = tipo TS que envuelve lo que lleva la acción (`action.payload`).
- Por qué `useAppDispatch` sobre `useDispatch`: tipado (thunk + sin cast manual). ✅

## ¿De dónde salen `state` y `action`? (respuesta del Profesor)

NO vienen del `initialState`. Son los **parámetros que Redux Toolkit inyecta** a cada reducer:

- `state` → estado **actual** del slice (el que va "acumulándose").
- `action` → objeto `{ type, payload }`.

Patrón Redux puro: cada reducer es `(state, action) => newState`.

## Estados asíncronos (patrón común del repo)

`idle → loading → success / error`

- `idle`: inicial, sin pedir aún.
- `loading`: petición en curso.
- `success`: respuesta OK (usa `action.payload`).
- `error`: fallo (red lenta, backend caído…).

## `initialState` tipado — el error "Unsafe assignment"

Conflicto al usar a la vez **anotación** (`: initialState`) **y** `satisfies FeedBackState`.
Regla: usar SOLO una de estas dos:

```ts
// Opción A (recomendada): anotar el tipo
const initialState: FeedBackState = { status: "idle", feedBack: [] }

// Opción B: satisfies (comprueba, no fuerza) — no lo mezcles con anotación
const initialState = { status: "idle", feedBack: [] } satisfies FeedBackState
```

- No usar `as FeedBackState` (el `as` fuerza y esconde errores de tipos).

## React.FC (Pregunta 2 de Diego)

- `React.FC` = `React.FunctionComponent`, un **tipo** TS para declarar componentes-función.
- `AuthProvider: React.FC<{ children: React.ReactNode }>`
  => componente que recibe `children` (lo que va dentro
  del JSX), tipado como `ReactNode` (cualquier cosa
  renderizable).

## Swagger UI — herramienta clave

- El back ya expone `GET /api-docs` (Swagger UI).
- Documenta **todos los endpoints y schemas** del backend automáticamente.
- URL dev: `https://ptd-talento-back-dev-17225193390.us-east1.run.app/api-docs/`
- **Uso:** entender el ecosistema (endpoints + schemas del ERD) sin leer todo el código.

## Flujo completo que Diego mapeó (✅ correcto)

hooks/useStore.ts (tipa useAppDispatch/useAppSelector)
-> hooks/useAuth.ts
-> api/auth.ts (axios)
-> util.ts (config VITE_BE_BASE_URL)
-> dispatch(...)

## Conclusión de Diego

>"Entendí la estructura del front, la importancia
>de Redux Toolkit, hooks, Context y el tipado en
>types. Redux es lo que conecta las interfaces con
>el payload que va al backend."
>"Redux: estado global centralizado; los slices son
>por entidad del ERD de Postgres."

---

## Mini-ejercicios pendientes / próximos

1. Corregir el bug del comentario de `Set` en `util/contador.ts` (`.size`, no `.length`).
2. (Opcional) Probar Opción A vs Opción B de tipado de initialState.
3. Rebajar uso de `as` en el proyecto (buscar con grep).
