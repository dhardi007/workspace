# Plan: Dual Portfolio — Diego Härdi

## Portfolio 1: `portfolio-terminal-dhardi` (Anónimo/Terminal)

### Issues a corregir

1. **Font 404s** — En `css/base.css` cambiar `@font-face` local por Google Fonts CDN (ya está linkeada en el `<head>`)
2. **Avatar** — En `index.html` línea 85, reemplazar `via.placeholder.com/80` por `assets/avatar/avatar-anom-tokyo-ken-touka-ghouls.jpg`
3. **Links de contacto** (líneas 315-335):
   - GitHub → `https://github.com/dizzi1222`
   - LinkedIn → `https://www.linkedin.com/in/dizzi-%C5%8Dkami-0b77093a1/`
   - Twitter → `https://x.com/dizzi_ds`
   - Dev.to → `https://dev.to/dizzi1222`
   - Email → `diegosamuel042@gmail.com`
4. **Iconos redes** — Agregar SVG inline con Simple Icons CDN junto a cada link
5. **Cross-link** — Footer: agregar enlace al portfolio comercial
6. **Copy hints** — Agregar comentarios sutiles tipo `# GitHub profile` en sección contacto

## Portfolio 2: `dhardi.dev` (Landing Comercial)

### Stack
- HTML + Tailwind CSS (CDN) + JS vanilla
- Sin frameworks pesados
- Hosting: GitHub Pages + Vercel

### Secciones

| Sección | Descripción |
|---------|-------------|
| **Hero** | Foto `avatar-dhardi-IMG_20250513_093622.jpg` (circular), nombre completo, tagline: *"DevOps & Software Engineer · Swiss in the Caribbean"* |
| **Sobre mí** | Suizo viviendo en RD, full-stack + Linux enthusiast, Neovim user |
| **Tech Stack** | Iconos SVG: Node.js, Express, React, MongoDB, Git, Arch, Neovim (mismos SVGs del terminal) |
| **Proyectos** | Dashboard React, Terminal Portfolio, Dotfiles, MCSD con links a GitHub |
| **Blog / Dev.to** | Sección preparada con consumo de API dev.to (`/api/articles?username=dizzi1222`). Placeholder mientras no haya artículos |
| **CV** | Botón descarga → `assets/Curriculum copium/[Diego Samuel - Cincinnatus](SIN FOTO) 85-curriculum-vitae-espanol.pdf` |
| **Contacto** | LinkedIn, GitHub, Twitter/X, Email profesional, formulario simple |
| **Footer** | Link "Versión Terminal 🖥️" + créditos |

### Paleta
- Heredar del terminal: `#e94560` (rojo), `#00d9ff` (cyan), `#0d0d0d` (fondo)
- Consistencia visual entre ambos portfolios

### Rutas de assets
- `assets/avatar/avatar-dhardi-IMG_20250513_093622.jpg`
- `assets/Curriculum copium/[Diego Samuel - Cincinnatus](SIN FOTO) 85-curriculum-vitae-espanol.pdf`
- Iconos SVG inline (mismos paths del terminal)

### Hosting
- GitHub Pages: `https://dizzi1222.github.io/dhardi.dev`
- Vercel: dominio personalizado opcional
- DNS: apuntar `dhardi.dev` o subdominio

## Enlaces cruzados
- Terminal → Footer: "Versión Comercial 🌐"
- Comercial → Footer: "Versión Terminal 🖥️"

## Orden de ejecución
1. Crear rama `feat/dual-portfolio` ✅
2. Arreglar `portfolio-terminal-dhardi` (fonts, avatar, links, icons)
3. Crear `dhardi.dev` desde cero
4. Deploy a GitHub Pages + Vercel
5. Publicar enlaces cruzados
