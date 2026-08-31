vim-learn: Resumen de la sesión — 31 de agosto (Aprendizaje continuo)

<img width="1926" height="1080" alt="image" src="https://github.com/user-attachments/assets/5741e376-5fe5-4a18-aefb-6d066014140b" />

<img width="1089" height="962" alt="imgi_2_a77699b0d4df7679c2e0c175e1a1ce27" src="https://github.com/user-attachments/assets/2562eda0-0bfe-4df0-9a47-2235bd5b7266" />

<img width="834" height="956" alt="image(1)" src="https://github.com/user-attachments/assets/99b189c6-db74-4110-857c-1924b4b43150" />

<img width="1100" height="901" alt="image(3)" src="https://github.com/user-attachments/assets/0bae7838-6596-40f0-a239-913cb2666dd4" />

— Lazygit + Neovim:
Desconocia por completo lazygit inicialmente, pensando que era solo otra herramienta de desarrollo. Pero descubrí que lazygit es una poderosa herramienta de navegación git dentro de neovim que, cuando se configura correctamente, permite un flujo de trabajo integrado manteniendo toda la potencia de mi configuración de zsh, incluyendo funciones personalizadas como gitflow, opencommit (commits automáticos) y aliases, mientras trabajo dentro del entorno de edición.

— Vim Motions — Reemplazos y selección:
viw Aprendí que para selección manual de remplazo: En viw, se usa el comando :s/pattern/replacement/ para reemplazos precisos en selecciones manuales, ideal para cambios puntuales sin afectar el resto del documento.
Selección múltiple con _: El comando_ busca la palabra actual bajo el cursor y la selecciona en todas sus ocurrencias, permitiendo luego usar :%s/pattern/replacement/ para reemplazos globales consistentes.
Tipos de remplazo: Existen varios tipos: remplazo simple (:%s/old/new/), remplazo con confirmación (:%s/old/new/gc), remplazo solo en líneas específicas (:%s/old/new/g), y remplazo con banderas especiales según necesidad.
Grug-Far Snacks Rename: Space + s + r
Para renombrar símbolos con precisión usando Grug-Far Snacks Rename, primero se selecciona el símbolo exacto con viw (usando v o V para selección visual), luego se aplica el rename para evitar cambios no deseados en comentarios o strings similares.
Remplazo profesional: Para reemplazos masivos y consistentes, primero se usa * para seleccionar todas las ocurrencias de una palabra, luego se aplica :%s/pattern/replacement/g para reemplazar todas las instancias de forma segura y eficiente.

📄 Ver vim-motions.md (<https://github.com/dizzi1222/workspace/blob/main/AGENT-records/vim-motions.md>)

— Redux Toolkit — Aprendizaje:
Se implementará un contador usando createSlice (que combina acciones y reducer), useSelector para leer estado, y useDispatch para disparar acciones, eliminando la necesidad de definir manualmente action types y reducers con switch.

📄 Ver ejercicio contador Redux (<https://github.com/dizzi1222/workspace/blob/main/AGENT-records/ptd-talento-front/src/store/slices/2026-08-31-ejercicio-contador-redux.md>)
📄 Ver sesión Redux anterior (<https://github.com/dizzi1222/workspace/blob/main/AGENT-records/ptd-talento-front/src/store/slices/2026-08-30-session-redux.md>)

Docker config para mongoup y MongoDB Compass GUI:
Se estableció "credsStore": "" en .docker/config.json para asegurar que Docker pudiera autenticar correctamente y permitir la instalación de mongoup sin conflictos, facilitando el uso de MongoDB Compass GUI para visualización y administración de bases de datos.

<img width="1436" height="806" alt="image(2)" src="https://github.com/user-attachments/assets/55eddbaa-9b9c-4438-8f5a-f6a79ddfcfc9" />

📄 Ver configuración Docker (<https://github.com/dizzi1222/dotfiles-dizzi/blob/main/.docker/config.json>) [No lo subo por seguridad ‼️]

— Otras referencias de aprendizaje:
Helix vs Neovim: desmitificando ventajas de Helix frente a LazyVim.
📄 helix-vs-neovim.md (<https://github.com/dizzi1222/workspace/blob/main/AGENT-records/2026-08-30-helix-vs-neovim.md>)

AuthContext (React.FC + Context ↔ Redux).
📄 AuthContext.tsx.md (<https://github.com/dizzi1222/workspace/blob/main/AGENT-records/ptd-talento-front/src/contexts/2026-08-30-AuthContext.tsx.md>)

useStore (useAppDispatch / useAppSelector).
📄 useStore.ts.md (<https://github.com/dizzi1222/workspace/blob/main/AGENT-records/ptd-talento-front/src/hooks/2026-08-30-useStore.ts.md>)

Express: patrón de rutas.
📄 express-rutas.md (<https://github.com/dizzi1222/workspace/blob/main/AGENT-records/ptd-talento-back/src/routes/2026-08-30-express-rutas.md>)

Plan: volver a MongoDB aprendiendo la "M" (ejercicio post-contador).
📄 PLAN-mongodb-migracion.md (<https://github.com/dizzi1222/workspace/blob/main/AGENT-records/PCE-Agencia/server/2026-08-31-PLAN-mongodb-migracion.md>)

Vim basics: fundamentos y trucos de comandos : (compilado del prepack LazyVim + docs nvim).
📄 vim-basics.md (<https://github.com/dizzi1222/workspace/blob/main/AGENT-records/vim-basics.md>)

Vim UI: mapa de todas las UIs/paneles que se abren en Neovim (Space+u toggles, Space+x quickfix, LazyGit, DAP).
📄 vim-ui.md (<https://github.com/dizzi1222/workspace/blob/main/AGENT-records/vim-ui.md>)

— Roadmap:
Continuo con el plan definido en workspace/learning-roadmap.md, enfocándome en integrar los aprendizajes de Redux Toolkit, Express routes y migración MongoDB en un proyecto completo, mientras exploro los recursos PTD-Talento especializados (Docker, React hooks, Redux) para enriquecer mi desarrollo full-stack.

📄 Ver roadmap de aprendizaje (<https://github.com/dizzi1222/workspace/blob/main/learning-roadmap.md>)

👾🔳 Spam de mi portfolio (builtin Svelte) porque no xd?: (<https://portfolio-terminal-dhardi.vercel.app/>)
👾🎨 NeonForge UI theme (proyecto de mi hermano): (<https://neonforge.ghaerdi.dev/> · <https://github.com/ghaerdi/neonforge>) — preset en `workspace/theme-neonforge.css`, candidato UI para el proyecto MongoDB/PCE-Agencia
👾 Best Neovim config: <https://github.com/dizzi1222/nvim>
👾📲 Best Termux Setup: (<https://github.com/dhardi007/dotfiles-dizzi/tree/termux>)
👾💻 Best NIX OS/Linux Dotfiles Config (<https://github.com/dizzi1222/dotfiles-dizzi>)

— Moraleja:
La ignorancia lleva al aprendizaje, el aprendizaje lleva a la solución, y la solución lleva a una mejor ignorancia (para el próximo problema a resolver).
