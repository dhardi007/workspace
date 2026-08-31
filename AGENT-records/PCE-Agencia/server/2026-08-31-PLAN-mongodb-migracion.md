# PCE-Agencia · Plan: volver a MongoDB (aprendiendo la "M" de MERN)

Sesión `vim-learn` · Plan de estudio post-contador · Proyecto: `PCE-Agencia/server`

> **Por qué este plan:** en la entrevista falló más por **falta de tiempo** que por error
> técnico: no alcanzó a **afirmar la "M" (MongoDB)** del stack MERN. Este ejercicio es,
> sobre todo, **aprender MongoDB de verdad** usando PCE-Agencia como material real,
> y de paso decidir si el server vuelve a Mongo (abandonando PostgreSQL).

## Contexto del proyecto (verificado 2026-08-31)

PCE-Agencia pasó por 2 ramas en `origin`:

| Rama | Base de datos | Estado |
|------|---------------|--------|
| `feat/mongodb-legacy` | **MongoDB (Mongoose)** — 7 modelos `*.model.js` | ⏪ la que tenía Mongo, se migró a Postgres |
| `feat/pg-prisma-api-robusta` | **PostgreSQL (Prisma)** — `prisma/schema.prisma`, `DATABASE_URL`, docker-compose con postgres | ✅ actual (`main`) |

La idea es **abandonar PostgreSQL y usar MongoDB completamente**, pero el punto central
es el **aprendizaje**: modelado de Mongo, diseño de colecciones, embebido vs referenciado.

## Material real disponible (para aprender)

- **Rama legacy Mongo**: `server/src/models/*.model.js` (Cliente, Proveedor, User,
  Transaccion, Itinerario, Factura, Reserva) + `server/src/config/db.js`.
- **Rama actual Prisma**: `server/prisma/schema.prisma` (mismos 7 modelos pero como tablas
  relacionales con relaciones).

## Por qué MongoDB ≠ PostgreSQL (lo que hay que interiorizar)

| PostgreSQL (relacional) | MongoDB (documental) |
|-------------------------|----------------------|
| Tablas con columnas y tipos | **Colecciones** con **documentos** BSON (sin esquema fijo) |
| Relaciones vía **JOIN / foreign keys** | Relaciones vía **embebido** o **referencias** (ObjectId) |
| `migrate` con SQL estricto | Mongoose **esquemas** en JS, sin migraciones pesadas |
| Datos normalizados por diseño | Diseño orientado a **cómo se lee** la app (denormalización ok) |

## Plan de lecciones (orden de estudio — es APRENDIZAJE de Mongo)

| Lección | Qué se aprende | Cómo (material real) |
|---------|----------------|----------------------|
| **MG-0** | Qué es un **documento** vs una fila; qué es BSON; qué es una colección | docs oficiales + comparar un doc de `Cliente.model.js` vs un `Cliente` de `schema.prisma` |
| **MG-1** | **Modelo mongoose**: `new mongoose.Schema(...)` + `model("Cliente", schema)` | leer los 7 `*.model.js` de la rama `mongodb-legacy` |
| **MG-2** | Tipos de campo (String/Number/Date/ObjectId), `required`, `unique`, `default` | `User.model.js` / `Transaccion.model.js` |
| **MG-3** | **Embebido vs referenciado**: cuándo anidar un subdocumento y cuándo guardar un `ObjectId` + `populate` | comparar `Reserva`/`Itinerario` (relaciones) en Mongo vs Prisma |
| **MG-4** | **CRUD con mongoose**: `find`, `findById`, `create`, `save`, `findByIdAndUpdate`, `deleteOne` | rutas de la rama legacy (clientes/proveedores/reservas) |
| **MG-5** | **`populate`** (el "join" de Mongo) y populate por `ref` | `Reserva.populate("itinerario")` etc. |
| **MG-6** | **Agregación** `aggregate([])` (pipeline: match/group/sort) — por qué Mongo lo resuelve distinto a SQL | ejercicio: total de facturas por cliente |
| **MG-7** | Conectar Nvim con MongoDB real (Compass + `mongosh` local o `mongodb-memory-server`) | `config/db.js` ya tiene fallback a memoria ✅ |
| **MG-x** | Decidir el **re-referenciado**: reusar `mongodb-legacy` o reescribir modelos aprendiendo de Prisma | decisión final con vim-ui/git |

## Regla de la práctica (vim-learn · Profesor)

- **No escribo código en el repo real** (PCE-Agencia incluido) — solo razonamiento + snippets
  de referencia en el chat. Diego los aplica.
- Cada lección: concepto → snippet de referencia → mini-ejercicio → verificación
  (correr/levantar server) → **Feynman** (Diego lo explica con sus palabras).
- El **RECORD** (nota de aprendizaje) se guarda acá, en `AGENT-records/PCE-Agencia/server/`,
  replicando la ruta del archivo real que se estudie (ej. `models/Cliente.model.js.md`).
  (Nombre con fecha: `2026-08-31-...` por convención.)

## Tema de UI (opcional — posible esquema de color)

Si el proyecto termina con UI, candidato de esquema de color: **NeonForge** (proyecto del
hermano de Diego → https://github.com/ghaerdi/neonforge · demo https://neonforge.ghaerdi.dev/).

- Archivo local: `workspace/theme-neonforge.css` (override de tokens de la página *Create*).
- ⚠️ **Gotcha:** NO es un tema standalone — es un *override* de custom properties. Debe
  **APPENDEARSE al final** del `theme.css` completo (el que trae `@theme inline`,
  `@utility` glass-panel / neon-glow / grid-bg). Si se usa solo, esas utilities desaparecen.
- Decisión pendiente: integrarlo en PCE-Agencia (lo más probable) o en un proyecto MongoDB nuevo.

Referencia de sesión completa: `workspace/motivational.md`.

## Relación con el resto del plan

- Es el **ejercicio posterior al contador** (que es ejercicio front/Redux). Este toca la
  **M y la persistencia del back**.
- Conecta con la sección **"Práctica Mermaid + MongoDB"** de `PLAN-ENTREVISTA.md`:
  el `erDiagram` (L3) de los modelos de PCE-Agencia sirve para **modelar en Mermaid** cómo
  deberían verse las colecciones de Mongo (embebido vs referenciado) antes de codear.

## Estado

- 🔲 MG-0 a MG-7 pendientes (orden de estudio de arriba).
- 🔲 Verificar rama `feat/mongodb-legacy` como material base (confirmado que existe con 7 modelos).
- 🔲 Decidir estrategia: reusar la rama legacy vs reescribir modelos aprendiendo (MG-x).
