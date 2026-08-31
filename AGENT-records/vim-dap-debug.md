# Vim DAP Debug — cómo usar el debugger (nvim-dap) correctamente

Sesión `vim-learn` · Compilado por el Profesor · Referencia continua (sin fecha).
Objetivo: **debuggear sin carpetazos**. Explica el flujo completo de nvim-dap, la
diferencia crítica entre **Launch** y **Attach** (la causa de tu error), y las configs
que ya tienes para Node y React/Chrome.

> Fuente: `dotfiles-dizzi/nvim/.config/nvim/lua/plugins/nvim-dap.lua` (config real).
> Debugger: `vscode-js-debug` (`pwa-node` / `pwa-chrome`) con adapter `server` de
> puerto fijo (53700) — se arranca `node vsDebugServer.js 53700` como job al cargar
> nvim-dap. `program` de "Launch file" usa la ruta ABSOLUTA del buffer (`expand("%:p")`)
> para que el breakpoint siempre case con el archivo ejecutado.
> ⚠️ Contexto: Diego abrió una vez "Attach" sin tener un proceso Node con `--inspect`
> y falló con "Could not connect to debug target at localhost:9229".

---

## 1. Concepto base — Launch vs Attach (lee esto primero)

Son **dos formas distintas** de debuggear, y confundirlas es el error más común:

| Modo        | Qué hace                                              | Requiere                                   |
| ----------- | ----------------------------------------------------- | ------------------------------------------ |
| **Launch**  | nvim **levanta** el programa con el debugger. Eliges "Launch file". | Nada más. Pones un breakpoint y `dc`. |
| **Attach**  | nvim **se conecta** a un programa que YA está corriendo con inspector abierto. | Un proceso ya iniciado con `--inspect[=puerto]`. |

> ⚠️ **Tu error:** elegiste **Attach** (o te tocó la de attach) y no había ningún Node
> escuchando en `9229` → "Could not find any debuggable target". El fix NO es config,
> es entender que **Attach** necesita un proceso previo con inspector.

**Regla práctica:**

- Quieres debuggear un script/archivo puntual → **Launch file**.
- Quieres debuggear una app ya corriendo (p.ej. un server levantado aparte) → **Attach**.

---

## 2. El flujo básico (Launch)

> ⚠️ **ATENCIÓN con los atajos (fuente de tu confusión):**
> - `Space + d + d` **NO es DAP** en tu setup — es el **dashboard** de LazyVim.
>   No tiene nada que ver con debuggear.
> - El menú de elegir config (**"Launch file"**) se abre con **`Space + d + c`**
>   (`continue()`), que la primera vez te muestra las configs para elegir.
> - `Space + d + a` es **"Run with Args"** (`da` → `continue({ before = get_args })`),
>   no estrictamente el menú genérico; igual te deja elegir "Launch file".
> - Resumiendo, las dos teclas que te llevan a seleccionar config y lanzar son
>   **`dc`** / **`da`**. `dd` = dashboard, no lo uses.

1. Abres el archivo a debuggear (`.ts`, `.js`, `.tsx`, `.jsx`).
2. Pones un breakpoint donde quieras pausar:
   - `<leader>db` — toggle breakpoint en la línea actual.
   - O `:lua require('dap').toggle_breakpoint()`.
3. Lanzas con **`<leader>dc`** (Continue) o **`<leader>da`**. Por primera vez **nvim te
   muestra un menú** con las configs disponibles para ese filetype — elige **"Launch file"**.
4. Al pausar: se resalta la línea en amarillo. Inspeccioná variables con **DAP UI**
   (`<leader>du`).
5. Avanzás con step over / into / out (ver sección 3).
6. Terminás con `<leader>dt` (Terminate).

---

## 3. Mapa de atajos (grupo `Space + D`)

| Atajo             | Acción                              | Equivalente que lanza                    |
| ----------------- | ----------------------------------- | ---------------------------------------- |
| `Space + D + d`   | ⚠️ **NO es DAP** — abre el DASHBOARD de LazyVim. No lanza nada. | — |
| `Space + D + u`   | **Toggle DAP UI** (panel variables / call stack / breakpoints / watch) | `:lua require('dapui').toggle()` |
| `Space + D + c`   | **Continue** (lanzar / continuar). La primera vez abre el menú para elegir **"Launch file"** | `require('dap').continue()`              |
| `Space + D + a`   | **Run with Args** — también abre el selector de config (elegí "Launch file") | `continue({ before = get_args })`        |
| `Space + D + b`   | **Toggle breakpoint** en esta línea | `require('dap').toggle_breakpoint()`     |
| `Space + D + B`   | Breakpoint **con condición**        | pide una condición                       |
| `Space + D + i`   | **Step into** (entrar a la función) | `step_into()`                            |
| `Space + D + o`   | **Step out** (salir de la función)  | `step_out()`                             |
| `Space + D + O`   | **Step over** (siguiente línea)     | `step_over()`                            |
| `Space + D + t`   | **Terminate** (matar la sesión)     | `terminate()`                            |
| `Space + D + r`   | **Toggle REPL** (consola del debug) | `repl.toggle()`                          |
| `Space + D + s`   | Session (ver sesión activa)         | `session()`                              |
| `Space + D + p`   | Pause                               | `pause()`                                |
| `Space + D + C`   | Run to cursor                       | `run_to_cursor()`                        |
| `Space + D + l`   | Run last (re-ejecutar última config)| `run_last()`                             |
| `Space + D + L`   | **Run with Args**                   | `continue({ before = get_args })`        |
| `Space + D + w`   | Widgets (hover)                     | `dap.ui.widgets.hover()`                 |
| `Space + D + g`   | Go to line (sin ejecutar)           | `goto_()`                                |
| `Space + D + k/j` | Up / Down en el call stack          | `up()` / `down()`                        |

> 💡 Con la **DAP UI** abierta (`du`) ves en tiempo real: variables, call stack,
> breakpoints y watch. Sin ella seguís pudiendo debuggear, pero sin el panel.

---

## 4. Configs que ya tienes (por filetype JS/TS/React)

Tus configs viven en `nvim-dap.lua` bajo `dap.configurations` para
`typescriptreact`, `typescript`, `javascript`, `javascriptreact`:

| Config (aparece en el menú) | Tipo                 | Cómo se usa                                          |
| --------------------------- | -------------------- | ---------------------------------------------------- |
| **Launch file**             | `pwa-node` (launch)  | Lanza el archivo actual con Node. Elige `program = ${file}`. |
| **Attach to process**       | `pwa-node` (attach)  | Se conecta a un Node que ya corre con `--inspect`.   |
| **Launch Chrome (React/Dev)**| `pwa-chrome` (launch)| Abre Chromium y debuggea tu app (Vite `localhost:5173`). |
| **Attach to Chrome**        | `pwa-chrome` (attach)| Se conecta a un Chromium abierto con `--remote-debugging-port=9222`. |

**Para Node (TS/JS puro):** usa **Launch file** — sin setup previo, solo breakpoints y `dc`.

**Para React (Vite):**

1. Levantá tu dev server: `npm run dev` (usa `localhost:5173`, si es otro puerto ajustalo en la config).
2. En un `.tsx`, `<leader>dc` → elige **Launch Chrome (React/Dev)**.
3. Pone breakpoints en el `.tsx` (con sourceMaps se mapean los archivos TSX).

---

## 5. Attach — cuándo y cómo (para no volver a fallar)

**Attach** se usa cuando el programa YA está corriendo y abriste un inspector:

```bash
# Caso Node: levantar el proceso CON inspector abierto
node --inspect=9229 tu-script.js
# o con --inspect-brk si quieres que pause al arrancar
node --inspect-brk=9229 tu-script.js
```

```bash
# Caso Chrome/React: levantar el navegador con el puerto de debugging
chromium --remote-debugging-port=9222
```

Entonces sí, en nvim: `<leader>dc` → elige **Attach** → elige el proceso/lin target.

> ⚠️ Si NO hay proceso con inspector en el puerto, Attach falla con
> "Could not connect to debug target". Asegurate siempre de que el proceso esté
> corriendo **antes** de Attach.

---

## 6. Errores comunes y qué significan

| Error / mensaje                                        | Causa y solución                                          |
| ------------------------------------------------------ | --------------------------------------------------------- |
| `No configuration found for 'typescriptreact'`         | Faltó la config por filetype. Ya agregada en `nvim-dap.lua`. |
| `Could not connect to debug target at localhost:9229`  | Elegiste **Attach** sin un proceso Node con `--inspect` arriba. Levantá el proceso antes, o usá Launch. |
| `Could not find any debuggable target`                 | Igual que el de arriba: no hay nada a lo que conectarse.  |
| `No thread to stop. Not pausing...`                    | No hay sesión activa / no hay hilos en pausa. Normal si nunca lanzaste. |
| `Debug adapter didn't respond`                         | El adapter tardó (esperá) o hay problema de config. Revisá `:help dap.set_log_level`. |
| `dap.ext.vscode.load_launchjs is deprecated`           | Ya no se usa; nvim-dap lee `.vscode/launch.json` solo. Eliminado de tu config. |
| `adapter.port is required for server adapter` (en `session.lua:1492`) | Es **de config**, no de uso. Aparece al seleccionar **"Attach to Chrome"** (`pwa-chrome` attach): nvim-dap espera que un adapter de tipo `server` tenga `port`, pero `vscode-js-debug` lo registra como executable y el `port = 9222` de la config no alcanza. **Fix:** mientras tanto, evitar "Attach to Chrome"; usar **Launch Chrome (React/Dev)** o **Launch file**. Verificar que el adapter esté registrado:
  ```lua
  :lua print(vim.inspect(require("dap").adapters["pwa-chrome"]))
  :lua print(vim.inspect(require("dap").adapters["pwa-node"]))
  ``` |

---

## 7. Flujo de referencia (cheat mínimo)

```vim
" Debuggear un archivo TS/JS (Node):
Space + db        " breakpoint en la línea (aparece el punto rojo)
Space + dc        " lanzar -> en el menú elige 'Launch file'
                  "    (Space + da también sirve; Space + dd es el DASHBOARD, NO DAP)
Space + du        " abrir DAP UI (variables / stack / breakpoints)
Space + dO        " step over
Space + di        " step into
Space + dt        " terminar

" Otros útiles:
Space + dC        " run to cursor (solo si YA está pausado en un breakpoint)
Space + d r       " toggle REPL
Space + d R?      " si el menú no aparece, revisá que el buffer sea .js/.ts/.jsx/.tsx
```

> ⚠️ `run_to_cursor can only be used if stopped at a breakpoint` → significa que la
> sesión está **corriendo sin pausar** (no se detuvo en ningún breakpoint). Si el archivo
> corre entero sin pausar pese a tener punto rojo, el breakpoint no casa con la ruta
> ejecutada (ver sección de arreglo `program` absoluto más abajo).

---

## Huecos detectados / por confirmar

1. El puerto de Vite (5173) y el de attach (9222/9229) están **hardcodeados** en las
   configs; si tu dev server usa otro puerto, hay que ajustarlo.
2. Confirmar si el Chromium se abre bien desde nvim (runtimeExecutable) o si conviene
   abrir el navegador aparte y usar **Attach to Chrome**.
3. Revisar si necesitas la extra de LazyVim de `dap` para Backend (Go, Python) si algún
   día debuggeas otro lenguaje.

---

## Referencias

- Config real: `dotfiles-dizzi/nvim/.config/nvim/lua/plugins/nvim-dap.lua`
- nvim-dap: <https://github.com/mfussenegger/nvim-dap>
- Opts del debugger: <https://github.com/microsoft/vscode-js-debug/blob/main/OPTIONS.md>
- LazyVim DAP: `:help dap` · `:help dap-configuration`
