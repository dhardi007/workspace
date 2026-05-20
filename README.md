# 🚀 Workspace — dizzi1222

Workspace con todos los proyectos clonados como submódulos.

## 📦 Proyectos

| # | Proyecto | Descripción |
|---|----------|-------------|
| 1 | `dizzi1222` | Perfil de GitHub |
| 2 | `Librezam` | — |
| 3 | `retro-portfolio` | Portfolio retro |
| 4 | `kimu-underground` | — |
| 5 | `portfolio-terminal-dhardi` | Portfolio estilo terminal |
| 6 | `GLAZE-WM-make-windows-pretty-main-dizzi` | Windows customización |
| 7 | `FCTicService.github.6c-Diego-05` | — |
| 8 | `REACT-Diego-Dizzi-Dashboard` | Dashboard en React |
| 9 | `Proyecto-App-MCSD` | App MCSD v2.0 |
| 10 | `dhardi.dev` | Landing page comercial |

## 🔧 Restaurar todos los proyectos

```bash
# 1. Clonar el workspace con submódulos
git clone --recurse-submodules https://github.com/dizzi1222/<repo>

# O si ya lo clonaste sin submódulos:
git submodule update --init --recursive

# 2. Ejecutar setup (clona, limpia y deja en main)
chmod +x setup.sh
./setup.sh
```

## ▶️ Iniciar proyectos localmente

### Portfolio Terminal (HTML estático)
```bash
cd portfolio-terminal-dhardi
python3 -m http.server 8080
# → http://localhost:8080
```

### Retro Portfolio (HTML estático)
```bash
cd retro-portfolio
python3 -m http.server 8081
# → http://localhost:8081
```

### REACT-Diego-Dizzi-Dashboard (React)
```bash
cd REACT-Diego-Dizzi-Dashboard
npm install
npm run dev
# → http://localhost:5173
```

### Proyecto-App-MCSD (Vite + Tailwind)
```bash
cd Proyecto-App-MCSD
npm install
npm run dev
# → http://localhost:5173
```

### dhardi.dev (Landing page comercial)
```bash
cd dhardi.dev
python3 -m http.server 8082
# → http://localhost:8082
```
