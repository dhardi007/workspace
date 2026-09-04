# AuthContext.tsx + React.FC — notas

Referencia real: `workspace/ptd-talento-front/src/contexts/AuthContext.tsx`

## Qué hace AuthContext

- Define un **Context** de React con el usuario autenticado (AuthUser) y métodos `loginCredentials` / `logout`.
- Se **sincroniza** con Redux:

  ```ts
  const dispatch = useAppDispatch()                    // 68 (escribe en el store)
  const reduxAuth = useAppSelector((s) => s.auth)      // 69 (lee del store)
  ```

- Un `useEffect` vigila `reduxAuth` y copia estado Redux -> Context local (`setUser`).

## ¿Por qué ambos (Context + Redux)?

- **Redux** = store global (fuente de verdad del estado de auth).
- **Context** = facilita leer el usuario en el árbol de componentes (Provider).
- Se sincronizan: dispatch (login/logout) escribe Redux; el effect refleja Redux en el Context.

## React.FC y children

```ts
export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {...}
```

- `React.FC<T>` = tipo de "componente función" que recibe props `T`.
- `children` = lo que va dentro del JSX `<AuthProvider>...aquí...</AuthProvider>`.
- `React.ReactNode` = tipo de "cualquier cosa renderizable" (JSX, string, number, null…).
