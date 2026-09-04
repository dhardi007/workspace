# Express: cómo armar una ruta (Fallo 3 de la entrevista)

Referencia real: `workspace/ptd-talento-back/src/`

## Confirmado: el back es EXPRESS + Node, NO NestJS

- `app.ts:3` →
  `import express, { json } from "express"`
- Estructura: `routes/` (express.Router) +
  `controllers/` + `Services/`
- Detectás NestJS por: `@nestjs/common`,
  decoradores `@Controller()/@Get()/@Module()`,
  `NestFactory.create()` — Diego NO tiene
  nada de eso.

## El patrón de montar rutas (su proyecto)

1. En `routes/xx.routes.ts`:

   ```ts
   const router = express.Router()
   router.get("/camino", handler)
   export const xxRouter = router
   ```

2. En `routes/index.routes.ts`:

   ```ts
   router.use('/api', xxRouter)   // lo monta bajo /api
   ```

3. En `app.ts:148`:

   ```ts
   app.use(indexRoutes)           // monta todas las rutas
   ```

Resultado: la ruta queda en `/api/camino`.

## Conexión front → back

- Front: `fetch(url, {method})` o `axios.post(url, body)` (ya usa axios en `api/auth.ts`).
- La URL base viene de `VITE_BE_BASE_URL` (ver `util.ts` de front).
- Back: recibe en el router -> controller -> service -> DB (TypeORM/Postgres).

## Pendiente visible del intento

`controllers/numeroContador.ts` quedó **vacío (0 bytes)** — es donde Diego iba a hacer
el contador back. Es el hueco natural para el ejercicio.

## Swagger UI ayuda a ver endpoints

`GET /api-docs` lista todos los endpoints y schemas del back automáticamente.
