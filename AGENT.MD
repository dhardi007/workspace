# AGENT.MD — Plan de rediseño de portafolios (review de hermano)

Aplica a **ambos** portafolios:
- `workspace/dhardi.dev` — https://dhardidev.vercel.app (Astro 7 + Tailwind v4)
- `workspace/portfolio-terminal-dhardi` — https://portfolio-terminal-dhardi.vercel.app (SvelteKit + Svelte 5, tema terminal)

> ⚠️ Convención de nombres: el repo SvelteKit se llama **portfolio-terminal**.
> "PTD" queda reservado para **PTD-Talento (CIC)** — proyecto aparte. Nunca llamar "PTD" al portfolio-terminal.

---

## Decisiones tomadas (con Diego)

| Tema | Decisión |
|---|---|
| LinkedIn único | `https://www.linkedin.com/in/diego-samuel-h%C3%A4rdi-santana-3a4343428` |
| Redes en HERO | Solo: LinkedIn, GitHub, Gmail, WhatsApp, Dev.to, Instagram |
| Redes en CONTACT | Todas (Twitter/X y Telegram permanecen ahí) |
| Línea Base hero | es: `Base República Dominicana (UTC-4)` · en/de: `Base Dominican Republic (UTC-4)` |
| Color "Open to work" | Cian existente de cada repo (`#00d9ff`). NO introducir `#05bbdc` |
| Badge Header | El verde emerald **"Disponible" del Header SE MANTIENE**; "Open to work" azul/cian es algo aparte que SOLO va en el hero |
| Idiomas | ~~A2~~ → **CORREGIDO**: `ES · EN (B2) · DE (A2)` — Diego tiene EN B2 (aplicado en dhardi.dev ×7 y portfolio-terminal ×3) |
| Slideshow hero dhardi.dev | Eliminar por completo (✅ hecho) |
| Línea nacido/pasaporte/vive | Fuera del hero → vive en el About Me completo |

## CORRECCIONES post-review (ronda 2)

1. **Cert CIC Associate Developer = 100%** en AMBOS repos: el desarrollo del proyecto PTD-Talento completa el requisito.
   - ✅ dhardi.dev: `.cert-bar-fill` width:100%, interval JS eliminado, tarjeta movida fuera de "En progreso", card consolidada con badge Completado + barra 100%, `cert-status`=100%, `#cert-text`="Completado ✓ — Proyecto PTD-Talento desarrollado", ES `placeholderDesc` sin "En proceso".
   - ⏳ portfolio-terminal: actualizar i18n `cert.internalStatus`/`waitingMessage` ×3 (`Certs.svelte`) a estado completado.
2. **Experiencia CIC** (ambos repos): Cincinnatus Institute of Craftsmanship, INC. — Proyecto **"Marketplace de Talento" [PTD-Talento]**, team size 4 devs, rol **Lead Tech**, coordinó/agilizó exitosamente el desarrollo, en espera de la pasantía. Período **Ene–Oct 2026**.
   - ✅ dhardi.dev: `exp.job4.*` ×3 idiomas + tarjeta primera del timeline (acento cian).
   - ⏳ portfolio-terminal: entrada en Experience.svelte.

## ESTADO ACTUAL

- ✅ **dhardi.dev**: FASE A completa y **COMITEADA** (`422ef81`): hero reescrito inline (sin slideshow, chips Base+Idiomas, Open to work cian + Download CV, 6 sociales), main `[Experience → TechStack → Proyects → Certification → AboutMe → Interests → Contact]`, servicios eliminados ×3, cert CIC 100%, exp.job4, SectionSetup → Intereses con imágenes webp, Footer LinkedIn unificado. Avatar2 corregido (rotado 180°, webp 220K regenerado + copiado a terminal).
- ✅ **dhardi.dev fixes Diego COMITEADOS**: `bf1d0af` nav responsivo en una línea (media queries 1180/1100/920px), avatar2 rotado; `01dc8cb` badge Future Interest con fondo translúcido original + letra blanca legible.
- ✅ **portfolio-terminal FASE B/C COMITEADA** (`8cc2896`): teclado vim completo + search overlay (`/`), 9 secciones `[~/ → ~/experience → ~/tech → ~/design → ~/projects → ~/interests → ~/certs → ~/about → ~/contact]` (1-9 saltan), Experience.svelte nuevo (timeline job4 cian primero), Interests.svelte nuevo, Hero chips+HUD 2 filas, About avatar2.webp sin born/lives/passport, NavBar/SectionNav hero→scroll top real, modal opaco (--bg-modal-solid + modalIn/backdropFade), Projects detailBlurIn, Certs CIC 100% (✓ verde, spinner eliminado), Design.svelte hover-follow tooltip como TechStack. check 0 errores / build verde.

## TANDA interests-v2 + fixes hero/about/certs (2026-08-23)

### dhardi.dev — Intereses v2
- Assets reorganizados por Diego en subcarpetas `public/interests/{gaming,anime,tv-series,music,photography,travel,drawing}/` (39 archivos). Normalizados a kebab-case: `drawing-kaneki-study.webp`, `ashita-no-joe-*`, `berserk-*`, `bleach-art-13.png`, `csm-art-*`, `dragon-ball-art-*`, `star-wars-best-of.mp4`. `travel5.webp` reescalada 150→1440px (Lanczos+unsharp, acepta blur).
- `SectionSetup.astro`: data model `img` → `imgs[]` (gaming ×8, anime ×12, tv ×1+video, music ×3, photography ×3, travel ×5, drawing ×2). Carrusel hover: interval 1500ms rotando `.is-active`, mouseleave reset. TV player: overlay ▶ al hover, click reproduce `star-wars-best-of.mp4` inline con sonido/controles, `ended` restaura imagen.
- `index.astro`: descripciones personales largas nuevas ×3 idiomas para gaming/anime/tv/music. **Sin mención a esposa/novia** (corrección de Diego).

### portfolio-terminal — mismo estándar + fixes review Diego
- Assets copiados íntegros a `static/interests/` (46 archivos con subcarpetas).
- `Interests.svelte`: mismo carrusel hover + TV player con runes ($state activeIdx/tvPlaying).
- `i18n/index.ts`: mismas 4 descripciones ×3 idiomas.
- `About.svelte`: restauradas filas born/lives/passport (🇩🇴 San Pedro de Macorís · Jarabacoa · 🇨🇭 Suizo) tal como estaban.
- `Hero.svelte`: más separación — chips fuera de la ventana; debajo fila centrada `hero-actions` con chip Base + botones GRANDES (Open to work cian glow + Descargar CV cian sólido) y `hero-socials` discretos (GitHub · Gmail · LinkedIn). HUD con margin-top gap-lg.
- `Certs.svelte`: resumen visible `cert-mini-section` con las 3 certificaciones no completadas (JSCAMP 20% · Mimo Full-Stack 15% · Exercism·TryHackMe 10%) con mini barras de progreso; detalle completo sigue en modal. Nueva clave i18n `cert.inProgress` ×3.

---

## FASE A — Ambos repos: Avatar + Interests

### A0. Avatar About Me → avatar2
- Cambiar imagen del **About Me** a `avatar2.jpg` (ya existe: `dhardi.dev/public/` y `portfolio-terminal-dhardi/static/`, 4.9MB).
- **Comprimir a WebP ~1200px (<300KB)** antes de wirear (pesa demasiado).
- Archivos: `AboutMe.astro:8` (dhardi.dev), `About.svelte:27` (terminal). Hero/glitch layers quedan con `avatar.png`.

### A1. Sección Interests estilo ghaerdi.dev (AMBOS repos)
Formato por tarjeta (copiar feel del sitio del hermano): **imagen → caption pequeña colorida ("game night") → título blanco → desc gris**.

Tarjetas personales de Diego:
- **Actuales**: Gaming · Anime & Manga · TV Film & Series (separado de Anime, no es lo mismo) · Music
- **Future Interest** (badge morado): Photography · Travel & Digital Drawing

Textos: adaptar a primera persona Diego (mismo tono que el hermano; los textos pegados son referencia).
Imágenes:
- Descargar los .webp de ghaerdi.dev a local (`public/interests/`, `static/interests/`):
  gaming.CX3c3xlk / anime.DNqaU-zW / music.BPaOKPWL / photography.CMbEYpeH / drawing.B7prhPUq / travel.BoXyvk0i
- TV Film & Series: buscar imagen cinematográfica CC0 nueva.
- Guardar textos fuente en el plan si hace falta re-consultar.

Tarjetas setup existentes (reconvertidas al MISMO formato, con imágenes):
- System Control → screenshot Rofi System Control de dotfiles-dizzi
- Scripts & Automation → Eww/Waybar o Fastfetch
- Galería del Setup → Arch Rice screenshot
- Blueprints & Tokens → mantener (o imagen asociada)
- Captions estilo Mr Robot (`./system_control.sh`, `cat waybar.log`…) inspiradas en https://dev.to/christinec_dev/lets-hack-the-world-in-the-mr-robot-ctf-4bj5
- Badges tech (Bash/Rofi/Zsh…) se conservan.
- Fuente imágenes setup: `/home/diego/dotfiles-dizzi/README.md` (GitHub user-attachments URLs).

### A2. Implementación
- **dhardi.dev primero**: reconstruir `SectionSetup.astro` (#interests) con grid de tarjetas nuevas + i18n ×3 (`setup.*` ampliado o nuevo `interests.*`).
- **portfolio-terminal después**: nueva sección `~/interests` (ventana terminal) mismo contenido adaptado + i18n ×3.

---

## FASE B — portfolio-terminal: Teclado + UX

### B1. HUD completo (Hero.svelte:114)
HUD añade: `B wallpaper │ ? keybindings │ t theme │ L lang │ h ← prev │ l → next │ I insert mode (form)` + segundo inline "More vim motions": `Enter next │ Backspace prev │ / search`.

### B2. Handlers (+layout.svelte:73)
- Existentes ya: `?`, `b`, `m`, `gg/G`, `j/k`, `Esc`.
- Nuevos: `h`=sección anterior, `l`=siguiente sección, `L`(shift)=cycleLang, `t`=toggleTheme, `I`→navega ~/contact + focus primer input del form, `Enter`=next sección, `Backspace`=prev, `/`=search overlay básico (salta a sección/proyecto encontrado).
- **Guard de forms**: si focus está en `input/textarea/[contenteditable]`, ignorar TODOS los atajos salvo `Escape` (que hace blur/sale del form). Al escribir nunca deben dispararse atajos.

### B3. NavBar `~/` al tope
El link `~/` debe hacer scroll REAL al top (`window.scrollTo({top:0})`) para que se vea la ventana `about_me.sh` con sus márgenes claramente (no `scrollIntoView` de #hero que queda tapado por header). Revisar también CSS scroll-margin-top.

### B4. Projects: blur-fade + modal legible
- **Animación blur-fade** en la imagen del detalle (`.detail-image`, Projects.svelte:298): de `blur(12px)+opacity 0+scale 1.02` a nítido on-open/on-load (mismo efecto "desenfoque" que dhardi.dev).
- **Modal opaco SIN toggle**: token nuevo `--bg-modal-solid` (`#121212` dark / `#fff` light) + override scoped en `.modal-window`. Decisión tomada: NO toggle button — opaco permanente resuelve legibilidad; ventanas normales siguen translúcidas.
- **Transición fluida estilo dhardi.dev**: entrada `scale(0.95) translateY(10px)→1` en 0.25s ease + backdrop fade (replicar keyframes `modalIn`/`modalBackdrop` de dhardi.dev global.css:42-43).

---

## FASE C — portfolio-terminal: Estructura

Orden final (definir exacto durante build, incluye lo pendiente):
`[~/ → ~/experience → ~/tech → ~/design → ~/projects → ~/interests → ~/certs → ~/about → ~/contact]`

- Nuevo `Experience.svelte` (ventana terminal `~/experience`) + datos:
  - Marketplace de Talento [PTD-Talento] — Cincinnatus Institute of Craftsmanship, INC. — Lead Tech, team 4 devs — Ene–Oct 2026 — badge Lead Tech (cian)
  - La Sirena — Asistente/Cajero — RD — 2025–2026 — badge Retail
  - Bootcamp Full-Stack Cincinnatus — RD — 2024–2026 — badge Education
  - Freelance MERN Stack Developer · DevOps — Arch Linux · Neovim — 2024–presente — badge Current
- Nuevo `Interests.svelte` (ver FASE A2).
- **Skills con hover informativo estilo dhardi.dev ("Tools of the trade")**: reutilizar el MISMO concepto de hover informativo de dhardi.dev para las skills del terminal:
  - `tech_stack.json // Tech Stack`: Express · React · MongoDB · PostgreSQL · Python · Git · Arch · Neovim · TypeScript · Docker · Tailwind CSS · Vercel · Google Cloud · Svelte · Astro · Railway · VoltBuilder · OpenCode
  - `design_portfolio.sh // Design`: Figma · Google Stitch · Design System · Prototyping · Canvas
  - El contenido de las descripciones YA EXISTE en i18n (`tech.*` y `design.hint`/`design.*` ×3 idiomas) — falta wirear el UI de tooltip/hover en `TechStack.svelte` y `Design.svelte`.
- Actualizar los **3 lugares** que definen secciones:
  1. `src/lib/stores/scroll.svelte.ts` (array + labels)
  2. Labels duplicadas `NavBar.svelte`
  3. Array hardcodeado + teclas `1..N` en `+layout.svelte`
- Cert CIC 100%: i18n `cert.internalStatus`/`waitingMessage` ×3 → estado completado (✓ Proyecto PTD-Talento desarrollado); revisar `Certs.svelte` para quitar "esperando".
- i18n ×3 idiomas (`exp.*`, `interests.*`).

---

## FASE D — CV + deploy (final)

1. Verificar render/descarga `/cv.pdf` en ambos. Diego edita el CV en Inkscape manualmente.
2. Build local ambos (`npm run build` / `pnpm build` según repo).
3. Deploy: `vercel --prod --yes` en cada repo.
4. **COMMITS sí, PUSH NO hasta que Diego confirme.**

## Orden de ejecución

1. AGENT.MD actualizado (este paso ✅)
2. dhardi.dev: FASE A (avatar + interests) → build → commit
3. portfolio-terminal: FASE B (hero restante + teclado + projects) → FASE A2+C (interests + experience + cert) → build → commit
4. FASE D deploy (sin push)

---

## Notas técnicas (no olvidar)

- dhardi.dev: hero está **inline** en `index.astro` (NO usar HeroLeft/HeroRight, están muertos). i18n dict inline `{es,en,de}` + `data-i18n`. Form validation usa `formI18n` separado.
- portfolio-terminal: store `lang` + helper `t(path)` por componente; localStorage `terminal-lang`.
- Vercel build: pnpm 10.28 + Node 24. `.npmrc` de portfolio-terminal debe mantener `engine-strict=false`.
- Deploy no interactivo: `vercel --prod --yes`.
- Nunca commitear `hypr/.config/hypr/scripts/text_animation/scripttext`.
- Compresión imágenes: verificar `cwebp`/ImageMagick disponible; fallback sharp vía npx.
- Breakpoints móviles: convención global XS **`≤ 480px`** (antes `450px`, luego `468px`). Nunca knife-edge en el ancho reportado por un dispositivo: Firefox Android reporta viewport ~468.x CSS px (DSF 2.3077 vs ~2.62 de Chromium en el mismo hardware) y fraccional → `(max-width: 468px)` no matcheaba aunque Eruda mostrara "468" (toggle invisible/duplicado). Terminal: `@media (max-width:480px)` + `innerWidth > 480`, toggle nav oculto con `min-width: 481px`.
- PTD-Talento front (MUI): breakpoints custom en `src/themes/main.ts` — xs `0` · sm `768` · md `1024` · lg `1440` · xl `1920`; sub-rango móvil ≤ 480px con `useMediaQuery("(max-width:480px)")`, detección móvil/tablet con `theme.breakpoints.down("md")`.
