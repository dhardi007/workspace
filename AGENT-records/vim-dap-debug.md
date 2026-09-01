# Vim DAP Debug — cómo usar el debugger (nvim-dap) correctamente

Sesión `vim-learn` · Compilado por el Profesor · Referencia continua (sin fecha).
Objetivo: **debuggear sin carpetazos**. Explica el flujo completo de nvim-dap, la
diferencia crítica entre **Launch** y **Attach** (la causa de tu error), y las configs
que ya tienes para Node y React/Chrome.

> Fuente: `dotfiles-dizzi/nvim/.config/nvim/lua/plugins/nvim-dap.lua` (config real).
> Debugger: `vscode-js-debug` (`pwa-node` / `pwa-chrome`). Los adapters `pwa-*` los provee
> el plugin **`nvim-dap-vscode-js`**, que lanza un `dapDebugServer` **por sesión con puerto
> dinámico** (reemplazó al viejo `server` de puerto fijo 53700, que era single-session y se
> colgaba → "comportamiento nulo" en JS/TS). `program` de "Launch file" usa la ruta ABSOLUTA
> del buffer (`expand("%:p")`) para que el breakpoint siempre case con el archivo ejecutado.
> ⚠️ Contexto: Diego abrió una vez "Attach" sin tener un proceso Node con `--inspect`
> y falló con "Could not connect to debug target at localhost:9229".
> ⚠️ **Bug E5108 arreglado (2026-09-01):** `preflight_check()` se llamaba ANTES de su
> declaración `local function`, por lo que `<leader>dc`/`<F9>` crasheaban con "attempt to
> call global 'preflight_check' (a nil value)" y NUNCA lanzaban la sesión DAP. Reordenado.

---

## 1. Concepto base — Launch vs Attach (lee esto primero)

Son **dos formas distintas** de debuggear, y confundirlas es el error más común:

| Modo       | Qué hace                                                                       | Requiere                                         |
| ---------- | ------------------------------------------------------------------------------ | ------------------------------------------------ |
| **Launch** | nvim **levanta** el programa con el debugger. Eliges "Launch file".            | Nada más. Pones un breakpoint y `dc`.            |
| **Attach** | nvim **se conecta** a un programa que YA está corriendo con inspector abierto. | Un proceso ya iniciado con `--inspect[=puerto]`. |

> ⚠️ **Tu error:** elegiste **Attach** (o te tocó la de attach) y no había ningún Node
> escuchando en `9229` → "Could not find any debuggable target". El fix NO es config,
> es entender que **Attach** necesita un proceso previo con inspector.

**Regla práctica:**

- Quieres debuggear un script/archivo puntual → **Launch file**.
- Quieres debuggear una app ya corriendo (p.ej. un server levantado aparte) → **Attach**.

---

## 2. El flujo básico (Launch)

> ⚠️ **ATENCIÓN con los atajos (fuente de tu confusión):**
>
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

| Atajo             | Acción                                                                                       | Equivalente que lanza                |
| ----------------- | -------------------------------------------------------------------------------------------- | ------------------------------------ |
| `Space + D + d`   | ⚠️ **NO es DAP** — abre el DASHBOARD de LazyVim. No lanza nada.                              | —                                    |
| `Space + D + u`   | **Toggle DAP UI** (panel variables / call stack / breakpoints / watch)                       | `:lua require('dapui').toggle()`     |
| `Space + D + c`   | **Continue** (lanzar / continuar). La primera vez abre el menú para elegir **"Launch file"** | `require('dap').continue()`          |
| `Space + D + a`   | **Run with Args** — también abre el selector de config (elegí "Launch file")                 | `continue({ before = get_args })`    |
| `Space + D + b`   | **Toggle breakpoint** en esta línea                                                          | `require('dap').toggle_breakpoint()` |
| `Space + D + B`   | Breakpoint **con condición**                                                                 | pide una condición                   |
| `Space + D + i`   | **Step into** (entrar a la función)                                                          | `step_into()`                        |
| `Space + D + o`   | **Step out** (salir de la función)                                                           | `step_out()`                         |
| `Space + D + O`   | **Step over** (siguiente línea)                                                              | `step_over()`                        |
| `Space + D + t`   | **Terminate** (matar la sesión)                                                              | `terminate()`                        |
| `Space + D + r`   | **Toggle REPL** (consola del debug)                                                          | `repl.toggle()`                      |
| `Space + D + s`   | Session (ver sesión activa)                                                                  | `session()`                          |
| `Space + D + p`   | Pause                                                                                        | `pause()`                            |
| `Space + D + C`   | Run to cursor                                                                                | `run_to_cursor()`                    |
| `Space + D + l`   | Run last (re-ejecutar última config)                                                         | `run_last()`                         |
| `Space + D + L`   | **Run with Args**                                                                            | `continue({ before = get_args })`    |
| `Space + D + w`   | Widgets (hover)                                                                              | `dap.ui.widgets.hover()`             |
| `Space + D + g`   | Go to line (sin ejecutar)                                                                    | `goto_()`                            |
| `Space + D + k/j` | Up / Down en el call stack                                                                   | `up()` / `down()`                    |

> 💡 Con la **DAP UI** abierta (`du`) ves en tiempo real: variables, call stack,
> breakpoints y watch. Sin ella seguís pudiendo debuggear, pero sin el panel.

---

### 3.1 Notificaciones fallback por lenguaje (build/requisito)

En `nvim-dap.lua` se agregó un sistema de avisos que, cuando el debugger **falla** o
**falta preparar el proyecto**, te dice el comando exacto según el lenguaje del buffer
en vez de un error criptico.

| Disparo                                                                                                                                                             | Qué hace                                                                                                                                                               |
| ------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **`<leader>dc` / `<F9>`** (continue con preflight)                                                                                                                  | Antes de lanzar hace `preflight_check()`: si falta el binario (Rust/C/C++) o el `.dll` (C#) avisa qué compilar; si es Java muestra el bloqueo. Luego `dap.continue()`. |
| **Fallo del adapter** (`output` con patrón conocido: "not a valid executable", "does not exist", "No module named debugpy", "adapter didn't respond", `0x80070002`) | Muestra el hint del lenguaje.                                                                                                                                          |
| **Evento `initialized` de sesión PHP**                                                                                                                              | Cuando arranca un "Listen for Xdebug" en PHP, recuerda proactivamente levantar el servidor con Xdebug (antes de `dc`).                                                 |
| **`<F8>`** (hint manual)                                                                                                                                            | Muestra el comando/requisito de build del lenguaje del buffer actual.                                                                                                  |
| **`<leader>dx`** (hint manual)                                                                                                                                      | Igual que `<F8>` pero accesible desde el grupo debug (`Space+d+x`).                                                                                                    |

> ⚠️ Se **descartó** interceptar `vim.notify` globalmente (rompía Noice: "vim.notify has
> been overwritten"). El aviso de Java sale del **preflight** en `<leader>dc`/`<F9>`, no de
> un wrapper global.

Los hints por filetype (se generan con el **directorio del buffer actual**, no al cargar el config):

| filetype  | Comando/requisito que sugiere                                                                                         |
| --------- | --------------------------------------------------------------------------------------------------------------------- |
| `rust`    | `cd <dir> && rustc -g -o build/main main.rs`                                                                          |
| `c`/`cpp` | `cd <dir> && g++ -g -o build/main main.cpp`                                                                           |
| `cs`      | `cd <dir> && dotnet build`                                                                                            |
| `java`    | DAP vía extra LazyVim `lang.java` (jdtls). Falta `:MasonInstall jdtls`; requiere JDK (hay 21).                        |
| `php`     | Levantar servidor con Xdebug antes de `dc` + `:MasonInstall php-debug-adapter` + **disparar request HTTP** (curl/URL) |
| `python`  | debugpy ya viene en `work.nix`; fallback `pip install debugpy`                                                        |

> El preflight corre en `<leader>dc` (sobrescribe el de LazyVim); los demás hooks coexisten.

---

## 4. Configs que ya tienes (por filetype JS/TS/React)

Tus configs viven en `nvim-dap.lua` bajo `dap.configurations` para
`typescriptreact`, `typescript`, `javascript`, `javascriptreact`:

| Config (aparece en el menú)   | Tipo                  | Cómo se usa                                                          |
| ----------------------------- | --------------------- | -------------------------------------------------------------------- |
| **Launch file**               | `pwa-node` (launch)   | Lanza el archivo actual con Node. Elige `program = ${file}`.         |
| **Attach to process**         | `pwa-node` (attach)   | Se conecta a un Node que ya corre con `--inspect`.                   |
| **Launch Chrome (React/Dev)** | `pwa-chrome` (launch) | Abre Chromium y debuggea tu app (Vite `localhost:5173`).             |
| **Attach to Chrome**          | `pwa-chrome` (attach) | Se conecta a un Chromium abierto con `--remote-debugging-port=9222`. |

**Para Node (TS/JS puro):** usa **Launch file** — sin setup previo, solo breakpoints y `dc`.

> ⚠️ En un `.js`/`.ts` suelto, al hacer `<leader>dc` el selector muestra Varias configs:
> hay que elegir **"Launch file"** (pwa-node), **NO** "Launch Chrome (React/Dev)" ni
> "Attach to Chrome". Elegir pwa-chrome da `Debug adapter didn't respond ... pwa-chrome`
> y intenta abrir Chromium (no es lo que quieres en un script Node suelto).

> **Fix "No thread to stop" (JS/TS):** en scripts cortos, `node ./index.js` corre entero
> en milisegundos y termina antes de que el breakpoint case (mensaje "Running …" que nunca
> pausa). La config **Launch file** de JS ahora usa `stopOnEntry = true`, así pausa en la
> primera línea ejecutable y luego los breakpoints sí casan al continuar.
> ⚠️ Esto es específico de **pwa-node (JS)**. En **codelldb (C/C++/Rust)** NO conviene
> `stopOnEntry` porque frena en `_start`/SIGSTOP (gotcha documentado en la § de C++).

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

| Error / mensaje                                                       | Causa y solución                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| --------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `No configuration found for 'typescriptreact'`                        | Faltó la config por filetype. Ya agregada en `nvim-dap.lua`.                                                                                                                                                                                                                                                                                                                                                                                                                         |
| `Could not connect to debug target at localhost:9229`                 | Elegiste **Attach** sin un proceso Node con `--inspect` arriba. Levantá el proceso antes, o usá Launch.                                                                                                                                                                                                                                                                                                                                                                              |
| `Could not find any debuggable target`                                | Igual que el de arriba: no hay nada a lo que conectarse.                                                                                                                                                                                                                                                                                                                                                                                                                             |
| `No thread to stop. Not pausing...`                                   | No hay sesión activa / no hay hilos en pausa. Normal si nunca lanzaste.                                                                                                                                                                                                                                                                                                                                                                                                              |
| `Debug adapter didn't respond`                                        | El adapter tardó (esperá) o hay problema de config. Revisá `:help dap.set_log_level`.                                                                                                                                                                                                                                                                                                                                                                                                |
| `dap.ext.vscode.load_launchjs is deprecated`                          | Ya no se usa; nvim-dap lee `.vscode/launch.json` solo. Eliminado de tu config.                                                                                                                                                                                                                                                                                                                                                                                                       |
| `adapter.port is required for server adapter` (en `session.lua:1492`) | Es **de config**, no de uso. Aparece al seleccionar **"Attach to Chrome"** (`pwa-chrome` attach): nvim-dap espera que un adapter de tipo `server` tenga `port`, pero `vscode-js-debug` lo registra como executable y el `port = 9222` de la config no alcanza. **Fix:** mientras tanto, evitar "Attach to Chrome"; usar **Launch Chrome (React/Dev)** o **Launch file**. Verificar que el adapter esté registrado con `:lua print(vim.inspect(require("dap").adapters["pwa-node"]))` |

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

## 8. Arreglos aplicados hoy (2026-08-31)

### 8.1 Adapter JS/TS: migrado de puerto fijo a `nvim-dap-vscode-js`

**ANTES (obsoleto):** se arrancaba `node vsDebugServer.js 53700` como **job con puerto FIJO**
(`jobstart detach`) y se registraban los adapters `pwa-*` como `server` en ese puerto:

```lua
dap.adapters["pwa-node"] = { type = "server", host = "127.0.0.1", port = 53700 }
dap.adapters["pwa-chrome"]       = dap.adapters["pwa-node"]
dap.adapters["pwa-msedge"]       = dap.adapters["pwa-node"]
dap.adapters["node-terminal"]    = dap.adapters["pwa-node"]
```

**PROBLEMA (diagnosticado 2026-09-01):** `vsDebugServer.js` es **single-session** con puerto
fijo. Si una sesión DAP quedaba colgada/ocupada, el server dejaba de responder a nuevos
clients → el DAP conectaba pero **no respondía al `initialize`** → "comportamiento nulo"
(`stopOnEntry` no cambiaba nada porque la nueva sesión ni conectaba).

**AHORA:** el plugin **`nvim-dap-vscode-js`** (dependency del spec de `vscode-js-debug`)
registra los adapters `pwa-*` y lanza un `dapDebugServer` **por sesión con puerto dinámico**,
parseando el stdout de forma robusta. Ya NO hay server global de puerto fijo.

```lua
-- En el spec de plugins de nvim-dap.lua
{
  "mxsdev/nvim-dap-vscode-js",
  lazy = true,
  dependencies = { "microsoft/vscode-js-debug" },
  config = function()
    require("dap-vscode-js").setup({
      debugger_path = vim.fn.stdpath("data") .. "/lazy/vscode-js-debug",
      adapters = { "pwa-node", "pwa-chrome", "pwa-msedge", "node-terminal" },
    })
  end,
}
```

> ⚠️ Usar **"Launch file"** (pwa-node), NO "Launch Chrome" (pwa-chrome da "adapter didn't
> respond" si no hay Chromium/DevTools). Al cambiar la config hay que **reiniciar nvim**
> (antes un server zombie del puerto 53700 podía quedar ocupándolo).

### 8.2 Sesión "running" que nunca pausa (breakpoint no casa) — RESUELTO

Con `program = "${file}"` y `cwd = "${workspaceFolder}"`, el debugger ejecutaba
`node ../../tmp/test.js` (ruta relativa) mientras el breakpoint se registraba con la ruta
**absoluta** del buffer → no coincidían → el archivo corría entero sin pausar, dando
`run_to_cursor can only be used if stopped at a breakpoint` y `No stopped threads`.

Fix: la config **"Launch file"** usa ruta absoluta y cwd en el directorio del archivo:

```lua
{
  type = "pwa-node",
  request = "launch",
  name = "Launch file",
  program = function()
    return vim.fn.expand("%:p")                    -- ruta ABSOLUTA del buffer
  end,
  cwd = function()
    return vim.fn.fnamemodify(vim.fn.expand("%:p"), ":h")  -- dir del archivo
  end,
  sourceMaps = true,
},
```

> 💡 Para validar que la sesión existe: `:lua print(require("dap").session())`
> devuelve un objeto (no `nil`) si hay sesión activa.

### 8.3 `No configuration found for javascript/typescript` + `E565: checktime` — RESUELTO

**Síntoma:** al pulsar `<leader>dc` (Continue) salía:

- `No configuration found for 'javascript'` / `'typescript'`
- y en el log de lazy: `Failed to run config for nvim-dap ... E565: Not allowed to
change text or change window: checktime ... nvim-dap.lua:306`

**Causa raíz:** en la config de Java dejé `mainClass = vim.fn.input(...)` como **llamada directa**,
cuando el resto de lenguajes (codelldb, C#, Go) usan `input()` **deferido dentro de una función**:

```lua
-- ❌ ROTO: ejecuta vim.fn.input() AL CARGAR (durante lazy-load con vim.schedule)
mainClass = vim.fn.input("Main class (ej. com.example.Main): ", "", "file")

-- ✅ CORRECTO: solo pide input cuando se lanza el debug de Java
mainClass = function()
  return vim.fn.input("Main class (ej. com.example.Main): ")
end
```

`<leader>dc` dispara nvim-dap por demanda (lazy-load). Durante ese `vim.schedule`,
llamar a `vim.fn.input()` está **prohibido** → `E565` → la carga de `config` se abortaba →
las configs de `javascript`/`typescript` (definidas en un `for` al final del mismo `config`)
**nunca se registraban**.

**Regla:** **nunca** poner `vim.fn.input()` como valor directo de un campo de configuración.
Siempre envolverlo en `function()`. Eso aplica a `mainClass`, `program`, `cwd`, `args`, etc.

### 8.4 Ejemplo C++ con codelldb (probado) — flujo + gotcha de `_start`

**Probar:** ejemplo en `/tmp/testcpp/main.cpp` (calculadora, build con `g++ -g -O0 main.cpp -o build/main`).

**Flujo** (igual al video de debugging C++/Neovim):

1. `nvim /tmp/testcpp/main.cpp`
2. `Space + d + b` → toggle breakpoint en una línea (ej. `int suma = sumar(...)`)
3. `Space + d + u` → abrir DAP UI (a la izquierda: Locals, Call Stack, Breakpoints; inferior: REPL/Console)
4. `Space + d + c` → menú → **"Launch file (codelldb)"**
5. Pregunta **"Path to executable:"** → `/tmp/testcpp/build/main` (apuntar al build dir, como el video)
6. Se detiene en el breakpoint. `Space + d + O` step over, `Space + d + c` continuar.
   - Locals muestra `x`, `y`, `suma`, `resta`, `mult` en vivo.
   - Le paso `-g` a g++ es **obligatorio**: sin debug info, codelldb no puede mapear el binario a tus líneas de fuente.

**Gotcha: parada en `_start` / `signal SIGSTOP` (NO es un crash)**

- **Síntoma:** al lanzar, el debugger frena en `_start` (no en `main`), muestra `Stop reason:
signal SIGSTOP`, registros (`rax`, `rbx`, `ymm0`...) y desensamblado (`movq %rsp, %rdi`),
  con `Source: unknown` y sin variables del programa.
- **Causa:** la config C++ tenía `stopOnEntry = true`. Eso obliga a codelldb a detener el
  proceso **justo al arrancar, en `_start`** (punto de entrada más bajo del binario, código
  del loader, antes de `main`). Ahí no hay fuente ni variables aún → parece un crash pero NO lo es.
- **Fix aplicado:** se quitó `stopOnEntry = true` de la config C++ (`nvim-dap.lua`). Ahora solo
  para en tus breakpoints reales, limpio como en el video.
- **Si aún quieres conservarla:** tras el stop en `_start`, `Space + d + c` salta a tu breakpoint.
- **`Console is in 'commands' mode, prefix expressions with '?'`** → no es error; es el REPL de
  codelldb en modo comandos (tipo gdb). Ignóralo.

---

---

## 9. 🧪 Práctica por lenguaje (archivos de test en `/tmp/`)

Cada test es una **calculadora** simple con operaciones en funciones — ideal para poner
breakpoints, step over/into y ver las variables (Locals) en la DAP UI. El flujo es siempre
el mismo:

> `nvim <archivo>` → `Space + db` (breakpoint) → `Space + dc` (lanzar, elegí config) → `Space + du` (DAP UI) → `Space + dO` (step over) / `di` (into) / `dc` (continuar) → `Space + dt` (terminar).

### 9.1 JavaScript/TypeScript — `/tmp/testjs/index.js` (pwa-node) ✅ sin build

```bash
# NO compila nada. Directo.
nvim /tmp/testjs/index.js
# <leader>db en una línea de console.log -> <leader>dc -> "Launch file"
```

- Inspección: panel **Locals** ya muestra `x`, `y` y el resultado de cada `console.log`
  cuando pausas. El **REPL** (`Space + dr`) evalúa con prefijo por filetype:
  `:x-y` (o en el REPL de pwa-node, `:exec x + y`). Recomendado usar **Watches**.
- **Si al lanzar ves `dap> node ./index.js` y luego `No thread to stop. Not pausing...`:**
  el programa `index.js` corrió del inicio al fin sin detenerse en tu breakpoint (es una
  calculadora que imprime y sale al instante). Causas:
  1. **Breakpoint en línea que no se alcanza** o el proceso ya terminó → verificar el punto rojo.
  2. **Ruta no casa** → confirmar que la config use `program = expand("%:p")` (absoluto) y
     `cwd` en el dir del archivo (ya está así en `nvim-dap.lua`).
  3. Si el puerto 53700 no escucha, el server no arrancó (ver sección 8.1). Verificalo con
     `ss -ltn | grep 53700` o `pkill -9 -f vsDebugServer.js` + reiniciar nvim.
     → Solución rápida: loguear tiempo suficiente o poner un breakpoint en una línea alcanzada y
     usar `pwa-node` Launch file; si sigue, revisar el log `:DapShowLog`.

### 9.2 C++ — `/tmp/testcpp/main.cpp` (codelldb) ✅ build listo

```bash
# Ya está compilado en build/main. Si cambias el .cpp:
# cd /tmp/testcpp && g++ -g -O0 main.cpp -o build/main
nvim /tmp/testcpp/main.cpp
# <leader>db en "int suma = ..." -> <leader>dc -> "Launch file (codelldb)"
# Pide "Path to executable:" -> /tmp/testcpp/build/main
```

- **Gotcha:** codelldb frena en `_start`/`SIGSTOP` si tu config tiene `stopOnEntry=true`
  (ya lo quitamos). Si vuelve a pasar, `dc` salta a tu breakpoint.
- REPL de codelldb está en modo **comandos** (tipo gdb, prefijo `?`). Para variables usa
  **Locals** o **Watches**.

### 9.3 Rust — `/tmp/testrust/main.rs` (codelldb) ⚠️ compilar primero

```bash
cd /tmp/testrust && rustc -g -o build/main main.rs   # build/ estaba vacío
nvim /tmp/testrust/main.rs
# <leader>db -> <leader>dc -> "Launch file (codelldb)" -> path: /tmp/testrust/build/main
```

- **Error típico sin build:** `Error on launch: '/tmp/testrust/main.rs' is not a valid executable`
  y `'/tmp/testrust/main' does not exist` — no había binario compilado, find_executable no
  halló nada. Solución: `rustc -g -o build/main main.rs` ANTES.
- Requiere `-g` (debug info). Igual que C++: usa **Watches**/Locals, no el REPL de comandos.

### 9.4 Go — `/tmp/testgo/main.go` (delve) ✅ sin build manual

```bash
nvim /tmp/testgo/main.go
# <leader>db -> <leader>dc -> elige "Launch" (delve compila el paquete solo)
```

- delve lanza con `program="${file}"` y compila él mismo. Si prefieres binario:
  `cd /tmp/testgo && go build -o build/main main.go` y usa la otra config.
- delve SI tiene REPL razonable; pero para ver variables usa **Watches** para ser consistente.

### 9.5 C# — `/tmp/testcs/Program.cs` (netcoredbg) ⚠️ compilar primero

```bash
cd /tmp/testcs && dotnet build            # genera bin/Debug/net8.0/*.dll
nvim /tmp/testcs/Program.cs
# <leader>db -> <leader>dc -> "Launch .NET" (hace glob de bin/Debug/**/*.dll)
```

- **Error típico sin build:** `Failed command 'configurationDone' : 0x80070002` (file not
  found) → el glob devuelve vacío porque no hay `.dll`. Solución: `dotnet build` ANTES.
- El adapter hace `glob("bin/Debug/**/*.dll")` — ya cubre el caso `net8.0/`. Si pide dll,
  apunta al `.dll` del build (no al `.cs`).

### 9.6 Java — `/tmp/testjava` (jdtls + java-debug-adapter) ⚠️ REQUIERE PROYECTO REAL

```bash
# El archivo a debugear DEBE estar dentro de un PROYECTO Java real.
# Formas válidas: pom.xml (Maven), build.gradle/.kts (Gradle), o markers Eclipse simple-project.
# /tmp/testjava usa markers Eclipse (ver abajo). jdtls importa el proyecto via LSP.
nvim /tmp/testjava/src/main/java/com/example/Main.java
# <leader>db -> <leader>dc -> "Launch Main Class" -> mainClass: com.example.Main
```

- **✅ MIGRADO a jdtls (2026-09-01):** el jar `com.microsoft.java.debug.plugin-0.53.2.jar` es un
  **bundle OSGi** que SOLO corre **DENTRO de jdtls** (runtime Eclipse/Equinox), no standalone
  (`java -jar` → "no main manifest attribute"). Ya NO se configura `dap.adapters.java` a mano.
  El adapter `java` ahora lo registra el **extra LazyVim `lang.java`** via
  `require("jdtls").setup_dap()`. **Requisitos runtime:** `:MasonInstall jdtls` + JDK (hay 21).
- **⚠️ "Main.java is a non-project file, only syntax errors are reported Java (16)":** jdtls trata
  un archivo suelto como NO-proyecto → análisis incompleto y NO permite DAP. Arreglado en
  `/tmp/testjava` con **markers Eclipse simple-project**: `.project` (natura javabuilder),
  `.classpath` (src `src/main/java`, JRE_CONTAINER **JavaSE-21**, output `bin`) y
  `.settings/org.eclipse.jdt.core.prefs` (compliance/source/target = 21). Opcional: borrar
  `pom.xml` si no hay Maven instalado (jdtls solo necesita un marker de proyecto válido).
- **⚠️ "Could not resolve java executable: Index 1 out of bounds for length 1":** falla jdtls al
  resolver el ejecutable de Java. En NixOS el JDK vive en `/nix/store/.../lib/openjdk` (layout no
  estándar) y `JAVA_HOME` suele estar VACÍO. Arreglado en `lua/config/options.lua`: al arrancar
  nvim, si `JAVA_HOME` está vacío se resuelve desde `exepath("java")` (dir que contiene `bin/
java`) y se setea `vim.env.JAVA_HOME`. Esto lo hereda el proceso jdtls al lanzarse.
- **⚠️ "Error on attach: Failed to attach to 127.0.0.1:5005 (attach timeout ...)":** es un
  **SÍNTOMA**, no la config real. Cuando jdtls NO reconoce el proyecto ("non-project"), su config
  provider devuelve vacío y nvim-dap cae a la config **estática** `dap.configurations.java` del
  extra LazyVim `lang.java`, que es `{ request = "attach", port = 5005, name = "Debug (Attach) -
Remote" }` (`~/.local/share/nvim/lazy/LazyVim/lua/lazyvim/plugins/extras/lang/java.lua:47-55`).
  Sin un debuggee escuchando en 5005 → falla el attach. **Causa raíz REAL (2026-09-01):** el config
  nuevo de jdtls usa `vim.lsp.config.jdtls.root_markers`
  (`~/.local/share/nvim/lazy/nvim-lspconfig/lsp/jdtls.lua`) que **NO incluye `.project`/`.classpath`**
  → solo reconoce `mvnw`/`gradlew`/`settings.gradle`/`.git` y `build.xml`/`pom.xml`/`build.gradle`.
  Por eso los markers Eclipse simple-project NO dete­ctan el proyecto como raíz. **Fix definitivo:**
  `/tmp/testjava` ahora tiene un `pom.xml` mínimo (Maven) + layout `src/main/java` (marker que jdtls
  SÍ reconoce). Reiniciar nvim DESDE la raíz (`nvim /tmp/testjava/src/main/java/com/example/Main.java`).
- **✅ "Launch Main Class" ahora es la UNICA config (default) — override `plugins/java-dap.lua`:**
  El extra LazyVim define `dap.configurations.java = { "Debug (Attach) - Remote" }` que confunde.
  Se creó `dotfiles-dizzi/nvim/.config/nvim/lua/plugins/java-dap.lua` que overridedemos `opts` de
  `mfussenegger/nvim-jdtls`: setea `dap` (fuerza `jdtls.setup_dap()` → adapter `java`), `dap_main
.on_ready` reordena `dap.configurations.java` dejando SOLO las configs `Launch *: *MainClass`
  (elimina la "Debug (Attach) - Remote"). Con una única config, `<leader>dc` la ejecuta directo =
  **default**, y `config_overrides = { stopOnEntry = true }` pausa en la 1ra línea (no "ejecuta
  instantáneo"). **⚠️ DOS GOTCHAS CRÍTICOS (2026-09-01):**
  1. **NO podemos devolver `opts` con solo `dap`/`dap_main`** → rompe el extra `lang.java`:
     `attempt to call field 'full_cmd' (a nil value)` y jdtls no arranca. El override DEBE replicar
     la estructura COMPLETA del `opts` del extra (root_dir, project_name, jdtls_config_dir,
     jdtls_workspace_dir, cmd, full_cmd, test, settings) y SOLO ajustar `dap`/`dap_main`.
     (`lua/plugins/java-dap.lua` ya está así).
  2. **`config_overrides` es una TABLA, NO una función** (`vim.tbl_extend('force', config,
config_overrides)` en `nvim-jdtls/lua/jdtls/dap.lua:371/649`). Pasar `function(config) return
config end` no agrega `stopOnEntry`. Usar `{ stopOnEntry = true }`.

### 9.7 PHP — `/tmp/testphp/index.php` (Xdebug) ⚠️ requiere php-debug-adapter + Xdebug

```bash
# 1) PREPARAR EL ADAPTER (una sola vez): php NO implementa DAP, hace falta el adapter de node
#    :MasonInstall php-debug-adapter

# 0) ⚠️ PREREQUISITO CRITICO: la extension Xdebug debe estar CARGADA en el PHP.
#    Comprobar:  php -m | grep xdebug        (debe listar 'xdebug')
#    Si NO aparece -> el debug NUNCA conecta (el adapter escucha en 9003 pero Xdebug no existe).
#    Causa real del error "thread stop no respuestas": Xdebug no instalado en este PHP (2026-09-01).
#    En NixOS se instala con un PHP construido con la extension, ej. en work.nix:
#      (php.withExtensions ({ enabled, all }: enabled ++ (with all; [ xdebug ])))
#    y luego `nixconf-rebuild` (home-manager). Sin el rebuild, usar `php -m` para confirmar.

# 2) PREPARAR EL SERVIDOR PHP con Xdebug ANTES de <leader>dc:
#    Servir con Xdebug en modo debug escuchando el puerto 9003:
cd /tmp/testphp
php -d xdebug.mode=debug -d xdebug.start_with_request=yes \
    -d xdebug.client_host=127.0.0.1 -d xdebug.client_port=9003 \
    -S localhost:8000
#    (deja esta terminal corriendo; es el "server" al que se conecta el adapter)

# 3) EN OTRA terminal de nvim: abrí index.php, breakpoint y lanza
nvim /tmp/testphp/index.php
# <leader>db en una línea -> <leader>dc -> elige "Listen for Xdebug" (port 9003)

# 4) DISPARAR una request HTTP: sin petición Xdebug NO conecta (aunque el server corra)
curl http://localhost:8000/index.php   # o navegá la URL que ejecuta el script
```

- **Orden clave:** (1) adapter, (2) servidor Xdebug **antes** de `dc`, (3) "Listen for Xdebug",
  (4) **request HTTP** (curl/abrir URL). El adapter espera la conexión de Xdebug; sin server
  da "Debug adapter didn't respond"; con server pero SIN request HTTP, Xdebug no conecta y
  el breakpoint no se dispara.
- **`pathMappings`**: se quitaron (el `["/var/www"] = cwd` viejo era incorrecto). Con el
  server built-in `php -S` corriendo desde el dir del archivo, PHP ve la ruta local real, así
  que no hace falta mapear si nvim abre el mismo archivo.

### 9.8 Python — `/tmp/testpython/main.py` (debugpy) ✅ configurado en Nix

```bash
# debugpy viene con el python de Nix (work.nix → python3.withPackages [.. debugpy]).
# Requiere un rebuild de home-manager para aplicar:
#   home-manager switch --flake ~/dotfiles-dizzi/nixconf

# Lanzar en nvim (después del rebuild):
nvim /tmp/testpython/main.py
# <leader>db en un print -> <leader>dc -> "Launch file (debugpy)"
```

- YA NO hace falta `pip install debugpy` a mano: work.nix incluye `debugpy` en el
  python env del PATH.
- El adapter detecta `.venv/bin/python` del proyecto si existe, si no el de Nix.
- Python SÍ tiene buen REPL de `dap>`: evaluá expresiones Python del frame directamente.

### 9.9 JS/TS y C# — fixes 2026-09-01

- **JS/TS — "Config references missing adapter `pwa-node`" RESUELTO.** El plugin
  `mxsdev/nvim-dap-vscode-js` (nvim-dap.lua) tenía `lazy = true` **sin trigger** → nunca se
  cargaba, y `dap-vscode-js.setup()` (que registra los adapters `pwa-node/chrome/msedge`) jamás
  corría. Fix: `event = "VeryLazy"` para cargarlo al inicio junto a nvim-dap y registrar los
  adapters antes de cualquier `<leader>dc`.
- **C# — "Failed command 'configurationDone' : 0x80070002" RESUELTO.** La config `program` y el
  `preflight` de C# usaban `vim.fn.getcwd()` (dir donde SE ABRIÓ nvim), pero el hint mostraba
  `buf_dir()` (dir del archivo). Inconsistencia → hint falso "No hay .dll" + netcoredbg lanzaba un
  path inválido → `0x80070002` (ERROR_FILE_NOT_FOUND). Fix: ambos usan ahora `buf_dir()`, así el
  dll se encuentra en `/tmp/testcs/bin/Debug/net8.0/testcs.dll` aunque nvim se abra desde otra
  carpeta.

### 9.10 JS/TS + PHP — adapters plugin + tsx + auto-open (2026-09-01)

- **JS/TS — consolidado en el plugin oficial `nvim-dap-vscode-js`.** Esta sesión se eliminó del
  `config` principal los adapters nativos `dap.adapters[pwa-*] = { type="server", port="${port}",
  executable={...} }` (que daban "No thread to stop" porque no entregan los breakpoints ANTES del
  launch). El `require("dap-vscode-js").setup({ ... })` ahora vive en el **spec `config`** del
  propio spec `mxsdev/nvim-dap-vscode-js` (con `requires = { "mfussenegger/nvim-dap",
  "microsoft/vscode-js-debug" }` y `debugger_path = stdpath(data).."/lazy/vscode-js-debug"`).
  El plugin lanza `vsDebugServer.js`, parsea el puerto del stdout e inyecta los breakpoints antes
  del launch → evita el "No thread to stop".
- **TS con tsx (decisión Diego).** Nueva config **"Launch TS (tsx)"** en
  `dap.configurations` para `typescript`/`typescriptreact`:
  `runtimeExecutable = "tsx"`, `runtimeArgs = { expand("%:p") }`, `cwd` ruta absoluta,
  `sourceMaps = true`. Patrón `runtimeExecutable`/`runtimeArgs` del README oficial de
  `nvim-dap-vscode-js` (igual a los ejemplos Jest/Mocha). Con tsx no hay compilación previa y
  pausa en la 1ra línea del `.ts`.
- **PHP — auto-open al continuar (decisión Diego).** `dap_continue_with_preflight` ahora, si
  `filetype == "php"` y `xdg-open` es ejecutable, lanza `vim.fn.jobstart({ "xdg-open",
  "http://localhost:8000/"..script })` ANTES de `dap.continue()`. Esto dispara la request HTTP que
  activa Xdebug (el adapter "Listen for Xdebug" solo escucha y necesita una petición al servidor
  built-in `php -S localhost:8000` para conectar). Ya no hace falta copiar/pegar el comando del
  hint.
- **C# — preflight corregido con tabla.** `preflight_check` para `cs` usaba
  `local dll = vim.fn.glob(...0,1)` (devuelve TABLA) y `if dll == ""` (nunca true, inútil). Fix:
  `local matches = vim.fn.glob(...0,1); local has_dll = type(matches)=="table" and #matches > 0`;
  avisa "compilar antes de debuggear" cuando `not has_dll`. El `program` C# también usa
  `matches[1]` (ya validado).
- **Validación:** `nvim --headless -u NONE -c "luafile nvim-dap.lua" -c "q"` → EXIT 0.
  Binarios confirmados en disco: `vscode-js-debug/out/src/vsDebugServer.js` ✓ y
  `nvim-dap-vscode-js/lua/dap-vscode-js/init.lua` ✓.

### Resumen estado de builds de los tests

| Test                      | Estado build      | Requisito previo                 |
| ------------------------- | ----------------- | -------------------------------- |
| `/tmp/testjs/index.js`    | ✅ listo          | ninguno                          |
| `/tmp/testcpp/main.cpp`   | ✅ `build/main`   | ninguno                          |
| `/tmp/testrust/main.rs`   | ⚠️ compilar       | `rustc -g -o build/main main.rs` |
| `/tmp/testgo/main.go`     | ✅ listo          | ninguno (delve compila)          |
| `/tmp/testcs/Program.cs`  | ⚠️ compilar       | `dotnet build`                   |
| `/tmp/testjava/Main.java` | ⚠️ compilar       | `javac -g -d out Main.java`      |
| `/tmp/testphp/index.php`  | ✅ (interpretado) | Xdebug activo en consola         |
| `/tmp/testpython/main.py` | ✅ (interpretado) | `pip install debugpy`            |

## Huecos detectados / por confirmar

1. El puerto de Vite (5173) y el de attach (9222/9229) están **hardcodeados** en las
   configs; si tu dev server usa otro puerto, hay que ajustarlo.
2. Confirmar si el Chromium se abre bien desde nvim (runtimeExecutable) o si conviene
   abrir el navegador aparte y usar **Attach to Chrome**.
3. **Python:** ✅ RESUELTO — debugpy ahora viene en `work.nix` (`python3.withPackages [.. debugpy]`); requiere `home-manager switch` para activar.
4. **Java:** ✅ MIGRADO — DAP vía extra LazyVim `lang.java` (jdtls). El bundle OSGi solo corre
   dentro de jdtls (`jdtls.setup_dap()`). Falta `:MasonInstall jdtls` (JDK 21 ya está).
5. **PHP:** ✅ config corregida — adapter `php-debug-adapter` + **paso de request HTTP** (curl/
   abrir URL) para que Xdebug conecte tras "Listen for Xdebug".
6. **JS/TS:** ✅ RESUELTO (2026-09-01) — el "comportamiento nulo" era el adapter `server` de
   puerto fijo 53700 (single-session, se colgaba). Migrado a `nvim-dap-vscode-js` (puerto
   dinámico por sesión). **Bonus:** arreglado el bug E5108 de `preflight_check` que impedía a
   `<leader>dc`/`<F9>` lanzar cualquier sesión DAP.

---

## Referencias

- Config real: `dotfiles-dizzi/nvim/.config/nvim/lua/plugins/nvim-dap.lua`
- nvim-dap: <https://github.com/mfussenegger/nvim-dap>
- Opts del debugger: <https://github.com/microsoft/vscode-js-debug/blob/main/OPTIONS.md>
- LazyVim DAP: `:help dap` · `:help dap-configuration`
