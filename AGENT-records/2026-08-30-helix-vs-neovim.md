# Helix vs Neovim — desmitificando las "ventajas" de Helix

Sesión `vim-learn` · Fecha: 2026-08-30 · Método: elaboración + analogía + contrademostración con tu propio setup

Contexto: un alumno de la charla de Carlos dijo "Helix es mejor que VSCode o Neovim".
Diego respondió: **no es mejor que mi vim actual (LazyVim)**. Esta nota lo argumenta.

---

## Qué es Helix

- Editor **modal** (como Vim/Neovim): teclas sobre ratón, modo normal/insert.
- Filosofía: "sel-less" — seleccionás con `v`/`x` y luego operás con `m` (multicursor nativo).
- **Ventaja real de Helix:** selección múltiple y edición estructurada son más fáciles de entrada.

## Por qué "mejor que Neovim" es FALSO (la desmitificación)

Que algo sea *más fácil al principio* no lo hace *más capaz*. Comparación honesta:

| Aspecto | Helix | Neovim (LazyVim de Diego) |
|---|---|---|
| Configuración | Menos config, "mejor default" | Configurable **al gusto** (Carlos: "configurar a gusto depende de cada quien") |
| Multiselección | Nativo (`x`), fácil de aprender | Se logra con `g/` + operadores, o plugins — igual de potente |
| Ecosistema/plugins | En crecimiento | **Gigante** (LazyVim trae cientos preconfigurados) |
| Lenguaje propio | Rust (rápido) | Neovim es Lua + Nvim API |
| Curva de entrada | Baja | Media, pero recompensa a largo plazo |
| Integración | Buena | Excelente (LSP, telescope, snacks, git) |

**Clave**: Helix atrae porque *no tenés que configurar nada*. Pero eso mismo es su techo:
no podés adaptarlo a tu flujo real. El setup de Diego (LazyVim) ya hace todo lo que Helix promete
(y más) una vez conocés los keymaps.

## Referentes en tu propio ambiente

- `dotfiles-dizzi/niri/.config/niri/exec-autostart.kdl`
  — tu entorno usa Niri + tooling de Neovim; Helix
  no entendería tu stack de ventanas/autostart como
  lo hace tu editor actual.
- `workspace/AGENT-records/vim-motions.md`
  — la chuleta vim que ya usás (Space+/ = live grep,
  Space+s+w). Helix reemplaza nomenclatura, no mejora
  tu velocidad real de búsqueda.
- `workspace/PCE-Agencia/package.json`
  — proyecto con server+client; Neovim + LSP
  (typescript-language-server) te da el tipado de
  `src/context/AuthContext.tsx` (interfaces TS).
  Helix no aporta ventaja acá; el LSP es el mismo.
- `workspace/dhardi.dev/src/components/ui/BlurFadeW.tsx`
  — componente de tu portafolio Astro: usando `*` +
  `n/N` y `Space+s+r` ya operás sobre `interface Props`
  con multiselección equivalente a la de Helix.
- `workspace/ptd-talento-front/src/store/slices/...`
  — tareas que hiciste hoy (Redux): todo con keymaps
  de LazyVim que ya dominás (grep, replace, mover
  bloques). Helix te haría re-aprender cada keymap
  sin darte poder nuevo.

## Conclusión

Helix es un **buen editor de entrada**, con multiselección fácil por defecto.
Pero "más fácil de arrancar" ≠ "más capaz". LazyVim de Diego es:

- Igual de potente en multiselección (Snacks + operadores).
- **Mucho más configurable** (filosofía que Carlos validó).
- Ya integrado en su flujo real (Niri, dotfiles, LSP, proyectos Astro/Svelte/React).

**Veredicto Profesor:** quedate con Neovim. No cambies de editor por una charla;
cambiala solo si tu editor te frena. A vos no te frena: te faltaba práctica, no el editor.

---

## Mini-ejercicio (Feynman)

> Explícame en 3 frases: ¿qué ventaja que crees que tiene Helix creés que tu LazyVim
> ya cubre, y con qué keymap concreto? (buscalo en `vim-motions.md` o en tu propia sesión).
