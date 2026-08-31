# Ejercicio: Contador con Redux Toolkit (`createSlice` + hooks)

Fecha: 2026-08-31 · Sesión `vim-learn` · Método: aprender haciendo (rol Profesor → Diego aplica)

> **Objetivo:** armar el contador que fallaste en la entrevista (IntelliSys 2026-08-26:
> solo hiciste una IIFE, sin estado ni UI), pero esta vez con **Redux Toolkit** (estado
> global real + UI). Entrena exactamente lo que te tumbó: **`createSlice` (Redux) +
> hooks (`useDispatch`/`useSelector`)** y que la **UI reaccione**.

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

## Paso 2 — conectar al store (`src/store/index.ts`)

```ts
import { configureStore } from '@reduxjs/toolkit';
import contadorReducer from './slices/contadorSlice';

export const store = configureStore({
  reducer: { contador: contadorReducer },
});
```

> Conservá lo que ya tengas de `auth` — agregá `contador` al objeto `reducer`.

## Paso 3 — componente con hooks (`src/components/Contador.tsx`)

```tsx
import { useDispatch, useSelector } from 'react-redux';
import { decrementar, incrementar, resetear } from '../store/slices/contadorSlice';

export default function Contador() {
  const valor = useSelector((state) => state.contador.valor); // LEE del store
  const dispatch = useDispatch();                             // DISPARA acciones

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
- `useSelector` **lee** (`state.contador.valor`).
- `useDispatch` **dispara** (`dispatch(incrementar())`).
- Esto es lo que se te escapó: **la IIFE sola no actualiza nada visual**. El store +
  dispatch + suscripción vía `useSelector` es lo que hace reaccionar la UI.

## Paso 4 — verificar Provider

El `<Provider store={store}>` debe envolver la app (`main.tsx` o `App.tsx`).
**Sin Provider, los hooks crashean.** Verificá que ya esté.

---

## Checklist de verificación / preguntas

- ¿Ya existe `src/store/index.ts` con `configureStore` y el `<Provider>` montado en `main.tsx`?
  Si no lo está → **ese es el paso que falta recordar** (useSelector crashea sin Provider).
- Archivos sueltos sin trackear (intentos previos): `createSlice.js`, `createSlice.ts`, `contador.ts`.
  ¿Los borramos al final o los dejamos como "intentos"? (decisión de Diego)

## Feynman (Diego explica tras hacerlo)

_(ocupar acá: "qué hace createSlice, qué hace useSelector/useDispatch, por qué sin Provider crashea")_
