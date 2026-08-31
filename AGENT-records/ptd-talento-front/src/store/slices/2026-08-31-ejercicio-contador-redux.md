# Ejercicio: Contador con Redux Toolkit (`createSlice` + hooks)

Fecha: 2026-08-31 · Sesión `vim-learn` · Método: aprender haciendo (rol Profesor → Diego aplica)

> **Objetivo:** armar el contador que fallaste en la entrevista (IntelliSys 2026-08-26:
> solo hiciste una IIFE, sin estado ni UI), pero esta vez con **Redux Toolkit** (estado
> global real + UI). Entrena exactamente lo que te tumbó: **`createSlice` (Redux) +
> hooks tipados (`useAppDispatch`/`useAppSelector`)** y que la **UI reaccione**.

Referencia real: `workspace/ptd-talento-front/src/store/` (patrón de `authSlice`).

---

## Paso 1 — slice (`src/store/slices/contadorSlice.ts`)

Por convención del repo va en `src/store/slices/` (mismo patrón que `authSlice`):

```ts
import { createSlice } from '@reduxjs/toolkit';

interface ContadorState { valor: number; }

const initialState: ContadorState = { valor: 0 };

const contadorSlice = createSlice({
  name: 'contador',
  initialState,
  reducers: {
    incrementar: (state) => { state.valor += 1; },  // mutación "segura" por Immer
    decrementar: (state) => { state.valor -= 1; },
    resetear:   (state) => { state.valor = 0; },
  },
});

export const { incrementar, decrementar, resetear } = contadorSlice.actions;
export default contadorSlice.reducer;
```

**Razonamiento (para entrevista):**
- `createSlice` genera **acciones + reducer juntos** (sin `action types` ni `switch` manual).
- `state.valor += 1` parece mutación, pero **Immer** hace la copia inmutable por ti → el
  "reducer puro" (debe devolver estado nuevo) se mantiene sin escribir spreads.

## Paso 2 — conectar al store (`src/store/store.ts`)

> ⚠️ El repo real NO usa `store/index.ts` sino **`store/store.ts`**, y ya tiene **muchos
> reducers** (auth, talent, etc.). Solo agregás el import y la entrada `contador` al
> objeto `reducer` existente (mismo naming: `ContadorReducer`, igual que `FeedBackReducer`).

```ts
// src/store/store.ts (agregar al archivo existente)
import ContadorReducer from './slices/contadorSlice';
// ...
export const store = configureStore({
  reducer: {
    // ...reducers existentes...
    contador: ContadorReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
```

> `RootState` / `AppDispatch` ya están exportados en `store.ts` y son lo que tipa los hooks.

## Paso 3 — componente con hooks tipados (`src/components/Contador.tsx`)

> ⚠️ El repo NO usa `useDispatch`/`useSelector` crudos. Usa los **hooks tipados**
> `useAppDispatch`/`useAppSelector` definidos en `src/hooks/useStore.ts`, que ya traen
> `RootState`/`AppDispatch` tipados (sin cast manual). Ver `2026-08-30-useStore.ts.md`.

```tsx
import { useAppDispatch, useAppSelector } from '../hooks/useStore';
import { decrementar, incrementar, resetear } from '../store/slices/contadorSlice';

export default function Contador() {
  const valor = useAppSelector((state) => state.contador.valor); // LEE del store (tipado)
  const dispatch = useAppDispatch();                             // DISPARA acciones (tipado)

  return (
    <div>
      <h2>Contador: {valor}</h2>
      <button onClick={() => dispatch(incrementar())}>+1</button>
      <button onClick={() => dispatch(decrementar())}>-1</button>
      <button onClick={() => dispatch(resetear())}>Reset</button>
    </div>
  );
}
```

**Razonamiento clave:**
- `useAppSelector` **lee** (`state.contador.valor`).
- `useAppDispatch` **dispara** (`dispatch(incrementar())`).
- **Por qué tipados:** `useAppSelector: TypedUseSelectorHook<RootState>` infiere el shape del
  estado y `useAppDispatch: () => AppDispatch` tipa las acciones (lo viste en la sesión 30-08).
- Esto es lo que se te escapó: **la IIFE sola no actualiza nada visual**. El store +
  dispatch + suscripción vía `useAppSelector` es lo que hace reaccionar la UI.

## Paso 4 — verificar Provider ✅ (ya está)

El `<Provider store={store}>` **ya envuelve la app** en `src/main.tsx` (importando `store`
desde `./store/store.ts`). No hace falta tocar nada; solo confirmar que sigue ahí.
**Sin Provider, los hooks crashean** — por eso `contadorSlice` y el componente dependen de él.

---

## Checklist de verificación / preguntas

- ✅ `src/store/store.ts` (NO `index.ts`) ya existe con `configureStore` y exporta `RootState`/`AppDispatch`.
- ✅ El `<Provider store={store}>` ya está montado en `main.tsx`. Si faltara → los hooks crashean.
- ✅ `src/util/contador.ts` (intento IIFE viejo) **borrado** 2026-08-31.
- ✅ Hooks tipados: `src/hooks/useStore.ts` (useAppSelector/useAppDispatch) — usar esos, no los crudos.

## Feynman (Diego explica tras hacerlo)

_(ocupar acá: "qué hace createSlice, qué hace useAppSelector/useAppDispatch, por qué sin Provider crashea")_
