# Plan de Estudio - JSCamp InfoJobs

## Estrategia

Seguir el bootcamp de midudev (JSCamp) recreando cada ejercicio con mis propias manos para afianzar conceptos. No persigo el certificado de pago (490€), sino el conocimiento real.

## Metodología

- **Fork propio** — `dizzi1222/jscamp` como submodule en el workspace
- **Ramas por lección** — Cada bloque de ejercicios en su propia rama, partiendo del punto exacto donde el instructor deja el ejercicio
- **Commits progresivos** — Reflejar mi proceso, no solo el resultado final
- **main** en el fork — copia fiel del `main` de midudev/jscamp (resultado final del bootcamp)

## Ramas planificadas

| Rama | Punto de partida | Descripción |
|------|------------------|-------------|
| `ejercicio-filtrando` | `9646b6b` | Base HTML/CSS estática + script mínimo |
| `leccion-change-event` | (desde ejercicio-filtrando) | Manejar evento change en JavaScript |
| `leccion-input-blur-submit` | (desde la anterior) | Más eventos y preventDefault |
| `leccion-fetch-json` | ... | Fetch y datos dinámicos |
| `leccion-web-components` | ... | Web Components |
| ... | ... | ... |

## Estructura del proyecto

```
workspace/
├── jscamp/                          # submodule → dizzi1222/jscamp
│   ├── 00-html-css/                 # Fundamentos HTML/CSS
│   ├── 01-javascript/               # JavaScript (estoy aquí)
│   ├── 02-react/                    # React
│   ├── 02-react-cdn-version/        # React via CDN
│   ├── 03-router-and-zustand/       # React Router + Estado global
│   ├── 04-express/                  # Backend con Express
│   ├── 04-node/                     # Node.js
│   ├── ...
│   └── 10-docker/                   # Docker
└── plan-de-estudio-Dhardi-dizzi1222.md
```

## Notas

- El bootcamp es 100% gratuito en [jscamp.dev](https://jscamp.dev)
- No hay dependencias externas de backend que requieran el repo original de midu
- Las ramas en el fork permiten mostrar mi progreso y recibir feedback
