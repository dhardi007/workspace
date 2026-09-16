# Supercomplete Test Project

Proyecto de prueba para toquetear con el Supercomplete/Tab (y para la sesión de
captura de la ruta B). TypeScript + React, con "costuras" donde el tab debería
sugerir, saltar e importar.

## Cómo usarlo

Abrí el IDE de Antigravity sobre esta carpeta y escribí sobre los marcadores
`PUNTO n`:

| Punto | Qué pasa | Dónde |
| ----- | -------- | ----- |
| 1 | Completar `normalizeUser` (email a minúsculas + tags) | `data.ts` |
| 2 | Completar el `reduce` de `stats` | `data.ts` |
| 3 | Completar el filtro de `visibleUsers` (q + filter) | `hooks/useUsers.ts` |
| 4 | Completar `formatLastSeen` (fecha) | `hooks/useUsers.ts` |
| 5 | Completar la tabla `<thead>/<tbody>` | `App.tsx` |
| 6 | **Tab-to-Import** `<UserCheck />` (lucide-react) | `App.tsx` |
| 7 | Salto tras aceptar (PUNTO 7) | `App.tsx` |

## Cómo correrlo

```sh
npm i typescript @types/react react 2>/dev/null
npx tsc --noEmit --jsx react-jsx --module esnext --moduleResolution bundler \
  --target es2022 App.tsx && echo "TS OK"
```
(No precisás instalar nada para probar el tab en el IDE: solo abrí la carpeta.)