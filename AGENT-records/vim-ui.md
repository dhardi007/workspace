# Vim UI — mapa de todas las interfaces/paneles (UIs) que se abren en Neovim

Sesión `vim-learn` · Compilado por el Profesor · Referencia continua (sin fecha).
Objetivo: saber **qué panel se abre con cada `Space + ...`**, para qué sirve, y
cuáles están "incompletas" o merecen un re-visitado.

> Fuente: `dotfiles-dizzi/nvim/.config/nvim/docs/neovim-keymaps.md` (keymaps reales de LazyVim).
> ⚠️ Contexto: Diego abre Neovim desde `~/`, así que ripgrep/Snacks barre todo el
> workspace salvo que límites Paths o hagas `:lcd`/`:cd` primero. Configura con `:pwd` o Ctrl + Alt + R path del proyecto.

---

## 1. Grupo `Space + u` — Toggles de UI (el grupo "U" de Diego)

`<leader>` = `Space`. Estos son los toggles visuales/UI:

| Atajo           | UI / Acción                  | Qué hace                                                                    |
| --------------- | ---------------------------- | --------------------------------------------------------------------------- |
| `Space + u + b` | Background                   | Alterna fondo claro/oscuro                                                  |
| `Space + u + C` | Colorscheme Picker           | Cambiar el tema de colores                                                  |
| `Space + u + d` | Diagnostics                  | **Alterna mostrar/ocultar los diagnostics** (errores del LSP)               |
| `Space + u + f` | Auto Format (global)         | True/off de auto-formato (global)                                           |
| `Space + u + F` | Auto Format (buffer)         | Auto-formato solo de este buffer                                            |
| `Space + u + g` | Indent Guides                | Guías de indentación                                                        |
| `Space + u + h` | Inlay Hints                  | Pistas de tipos en línea (TS)                                               |
| `Space + u + I` | **Inspect Pos (Treesitter)** | Inspecciona el nodo bajo el cursor (arbol sintáctico — "el DOM" del buffer) |
| `Space + u + l` | Line Numbers                 | Números de línea on/off                                                     |
| `Space + u + L` | Relative Numbers             | Números relativos on/off                                                    |
| `Space + u + n` | Dismiss Notifications        | Cerrar las notificaciones                                                   |
| `Space + u + s` | Spelling                     | Corrector ortográfico on/off                                                |
| `Space + u + k` | Screenkey                    | Mostrar las teclas que pulsás (para grabar demos)                           |
| `Space + u + T` | Treesitter Highlight         | Resaltado de sintaxis Treesitter on/off                                     |
| `Space + u + w` | Word Wrap                    | Ajuste de línea on/off                                                      |
| `Space + u + z` | Zen Mode                     | Modo zen (limpiar la UI, solo el texto)                                     |
| `Space + u + Z` | Zoom                         | Zoom sobre la ventana actual (legibilidad)                                  |

> Nota de Diego: `Space + U + W` (legibilidad con opencode) → eso es en realidad
> **Zoom** (`Space + u + Z`) o el **Zen** (`Space + u + z`). Verificá cuál era.

## 2. Grupo `Space + N` — Notifications

| Atajo           | UI            | Qué es                                             |
| --------------- | ------------- | -------------------------------------------------- |
| `Space + N`     | Notifications | Centro de notificaciones (= `:mes` / `:messages`). |
| `Space + u + n` | Dismiss       | Cerrar/dimirir las notificaciones pendientes       |

## 3. Grupo `Space + x` — Diagnostics & Quickfix (Trouble)

| Atajo           | UI                              | Qué es                                       |
| --------------- | ------------------------------- | -------------------------------------------- |
| `Space + x + x` | Document Diagnostics (Trouble)  | Errores/warnings del **documento actual**    |
| `Space + x + X` | Workspace Diagnostics (Trouble) | Errores de **todo el workspace**             |
| `Space + x + L` | Location List (Trouble)         | Lista de ubicaciones                         |
| `Space + x + Q` | **Quickfix List (Trouble)**     | La quickfix UI (resultados de vimgrep, etc.) |

Navegación rápida de diagnostics/quickfix:

| Tecla       | Acción                          |
| ----------- | ------------------------------- |
| `[d` / `]d` | Diagnostic anterior / siguiente |
| `[e` / `]e` | Error anterior / siguiente      |
| `[w` / `]w` | Warning anterior / siguiente    |
| `[q` / `]q` | Quickfix anterior / siguiente   |

## 4. LazyGit (Git UI) — `Space + G`

> **`Space + G + G`** entra directo a la **Status** de LazyGit (lo mejor de LazyVim):
> blame (autor de cada línea), diff, contexto completo de cada cambio, navegar
> ramas/stash/commits, sin salir de nvim.

| Atajo           | UI / Acción | Qué es                                               |
| --------------- | ----------- | ---------------------------------------------------- |
| `Space + G + G` | **Status**  | Panel principal de LazyGit — blame + diff + contexto |
| `Space + G + i` | Open        | Abrir LazyGit en el repo actual                      |
| `Space + G + I` | All         | Abrir LazyGit en **todos** los repos/working dirs    |
| `Space + G + s` | Status      | Abrir el panel status                                |
| `Space + G + S` | Stash       | Abrir stash                                          |
| `Space + G + Y` | Copy        | Copiar hash/commit seleccionado                      |

## 5. DAP (Debug) — `Space + D`

| Atajo           | UI / Acción       | Qué es                                                                      |
| --------------- | ----------------- | --------------------------------------------------------------------------- |
| `Space + D + u` | **Toggle DAP UI** | Abre/cierra la UI de depuración (variables, call stack, breakpoints, watch) |
| `Space + D + c` | Continue          | Ejecutar/continuar el debug                                                 |
| `Space + D + b` | Toggle breakpoint | Poner/quitar breakpoint                                                     |

> ⚠️ DAP puede pedir config por lenguaje (`dap.configurations.<lang>`).
> `dap.ext.vscode.load_launchjs` ya no hace falta: los `.vscode/launch.json` se leen solos.

## 6. Recuperar ventanas / buffers (`:ls!` y `:b<num>`)

**El caso de Diego:** a veces cambia de "window" (o se abre un panel/otro buffer) la
ventana de Opencode /el buffer anterior **desaparece de la vista**, pero NO se cerró:
quedó como buffer **oculto** (hidden) en Neovim.

| Comando             | Qué hace                                           |
| ------------------- | -------------------------------------------------- |
| `:ls`               | Lista los buffers abiertos (con número)            |
| `:ls!`              | Lista **todos** los buffers, incluidos los ocultos |
| `:b7`               | Saltar al buffer **número 7** (el que elegiste)    |
| `:b <num>`          | Saltar al buffer número `num`                      |
| `:b <nombre>`       | Saltar a un buffer por fragmento de nombre         |
| `:bnext` / `:bprev` | Buffer siguiente / anterior (o `:bn` / `:bp`)      |

**Flujo para recuperar lo que "desapareció":**

```vim
:ls!        " ver todos los buffers con su número (aunque estén ocultos)
:b7         " volver al buffer 7 (o el que quieras del listado)
```

> 💡 Si el buffer estaba en otra **ventana** (split) y solo cerró la ventana pero el
> buffer sigue, `:ls!` lo muestra y `:b<num>` lo reabre en la ventana actual.
> Los buffers marcados con `%` = buffer actual, `#` = buffer alterno (podés ir con `Ctrl+^`).

---

## Huecos detectados (faltan por resolver/documentar)

1. **Diagnostic log**: con `Space + u + d` alternás el _mostrar_ los diagnostics, pero
   **no hay una UI para ver el log COMPLETO de errors ni copiarlo**. Dónde se guarda y
   cómo copiarlo a un buffer/archivo es el hueco que anotó Diego ("FALTA UI").
   - Buena fuente: `Space + x + x` (Trouble diagnostics) muestra la lista navegable.
   - Para copiar todo el log de errors: se puede usar `:messages` / Buffer Diagnostics.
2. Verificar si `Space + U + W` era **Zoom** o **Zen** (a confirmar con Diego).
3. Revisar el grupo `<leader>G` completo (hay más acciones LazyGit de las listadas).

---

## Referencias

- Keymaps reales: `dotfiles-dizzi/nvim/.config/nvim/docs/neovim-keymaps.md`
- LazyGit: <https://github.com/jesseduffield/lazygit>
