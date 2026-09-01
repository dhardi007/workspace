# Vim Mason — instalación de LSPs, DAPs, linters y formatters

Sesión `vim-learn` · Compilado por el Profesor · Referencia continua (sin fecha).

Objetivo: entender **qué instala Mason**, cómo se **disparan las auto-instalaciones** de
adapters DAP, y qué **otras herramientas** conviene agregar según el roadmap de lenguajes.

> Fuente real: `dotfiles-dizzi/nvim/.config/nvim/lua/plugins/mason.lua` y
> `dotfiles-dizzi/nvim/.config/nvim/lua/plugins/nvim-dap.lua`.

## 1. Cómo abrir Mason (atajo)

| Acción                        | Atajo LazyVim        | Comando      |
| ----------------------------- | -------------------- | ------------ |
| Abrir Mason (TUI de paquetes) | `Space + c + m`      | `:Mason`     |

Dentro de Mason:

- `1/2/3/4` filtran por **All / LSP / DAP / Linter / Formatter**.
- `<C-f>` abre el **Language Filter**: escribís una extensión de lenguaje (p. ej. `cpp`, `py`,
  `go`) y **`<Enter>` la aplica** → Mason muestra solo los paquetes de ese lenguaje. Útil para
  ver qué LSP/DAP/linter hay disponible para C++ o cualquier lenguaje sin hacer scroll.
  (El prompt dice: _press `<C-f>` to apply filter_; limpiás el filtro con `<C-f>` + borrar o
  `l` para limpiar).
- `g?` abre la ayuda de keymaps internos.
- `i` instala el paquete bajo el cursor, `x` lo desinstala, `C-u` lo actualiza.

## 2. Por qué NO se autoinstalaron los adapters DAP al abrir nvim

En `nvim-dap.lua` está el `ensure_installed` de `mason-nvim-dap`:

```lua
if LazyVim.has("mason-nvim-dap.nvim") then
  require("mason-nvim-dap").setup(
    vim.tbl_deep_extend("force", LazyVim.opts("mason-nvim-dap.nvim"), {
      ensure_installed = {
        "codelldb",           -- Rust / C / C++
        "delve",              -- Go
        "netcoredbg",         -- C# (.NET)
        "java-debug-adapter", -- Java
        "php-debug-adapter",  -- PHP (Xdebug)
      },
      automatic_installation = true,
    })
  )
end
```

**Causa real:** el `setup()` (y por tanto el `ensure_installed`) **solo corre cuando el
plugin `nvim-dap` se carga por demanda** (lazy-load en LazyVim). `nvim-dap` no se carga al
abrir nvim: hay que dispararlo con un keybinding de debug (`Space + d...`) o al exigirlo
un lenguaje. Si al abrir no tocaste nada de debug, el `ensure_installed` jamás se ejecutó.

> ⚠️ El `mason.lua` **no es la causa**: solo declara `mason.nvim` y `mason-lspconfig.nvim`
> sin `ensure_installed`. Los DAPs van por `mason-nvim-dap`, que ya viene como dependencia
> de nvim-dap (está en `lazy-lock.json`).

**Solución aplicada:** el `ensure_installed` de `mason-nvim-dap` ya se disparó (al cargar
`nvim-dap` por demanda) y dejó los 5 adapters en **Installed** ✅. Confirmado en la lista Mason:

```
◍ codelldb
◍ delve
◍ java-debug-adapter
◍ netcoredbg
◍ php-debug-adapter
```

> ⚠️ Mason es **por-máquina**: los paquetes viven en `~/.local/share/nvim/mason/` y persisten
> entre sesiones del mismo equipo, pero **NO se replican** a otras PCs. Cada máquina dispara
> su propio install cuando cargas nvim-dap.

> 💡 **¿Y por qué veo "Installing ◍ cpptools" al abrir nvim?** Eso **no** viene del `ensure_installed`
> de DAPs (npm-dap → solo codelldb/delve/netcoredbg/java-debug/php-debug). `cpptools` es un **LSP**
> y lo instala **`mason-lspconfig`** (LazyVim auto-agrega LSPs a su `ensure_installed` según los
> lenguajes/formatters que detecta en tu config). A Java se vio "Downloading LSP configuration
> schema from `vscode-cpptools/.../package.json`" = cpptools descargando su schema al compilar.
> Es **normal y esperado**: es un segundo `ensure_installed` distinto (el de LSPs).

## 3. Paquetes DAP instalados (adapters por lenguaje)

| Adapter             | Lenguaje(s)        | Binario en Mason      | Estado              | Config en nvim-dap.lua |
| ------------------- | ------------------ | --------------------- | ------------------- | ---------------------- |
| `codelldb`          | Rust / C / C++     | `mason/bin/codelldb`  | ✅ Funciona | adapter `server` puerto 13000 |
| `delve`             | Go                 | `dlv`                 | ✅ Funciona | adapter `go` con spawn `dlv dap` |
| `netcoredbg`        | C# / VB / F#       | `mason/bin/netcoredbg`| ✅ Funciona | executable con `--interpreter=vscode` |
| `java-debug-adapter`| Java               | (bundle dentro de `jdtls`) | ✅ Funciona vía jdtls | extra LazyVim `lang.java` → `jdtls.setup_dap()` |
| `php-debug-adapter` | PHP (Xdebug)       | `mason/bin/php-debug-adapter` | ✅ Instalado (requiere Xdebug) | executable, puerto 9003 |
| `pwa-node`          | JS/TS (Node)       | (viene de `vscode-js-debug`) | ✅ Funciona | adapter `pwa-*` vía plugin `nvim-dap-vscode-js` |

### ✅ Java ahora vía jdtls (migrado)

El jar de `java-debug-adapter` NO se ejecuta standalone: es un **bundle OSGi** que SOLO corre
**DENTRO de jdtls** (runtime Eclipse/Equinox). Por eso el viejo `java -jar <bundle>` daba
`command java exited 1`. La vía correcta (migrada) es el **extra LazyVim `lang.java`**:

- Se activa en `lua/config/lazy.lua` con `{ import = "lazyvim.plugins.extras.lang.java" }`.
- `nvim-jdtls` arranca `jdtls` (LSP de Java) que carga el bundle java-debug como bundle OSGi.
- `require("jdtls").setup_dap()` registra el adapter `java` en nvim-dap.
- **Requisitos runtime:** `:MasonInstall jdtls` + JDK instalado (Diego tiene OpenJDK 21, coincide
  con el `Build-Jdk-Spec 21` del bundle). jdtls debe estar **corriendo** al debugear.

> PHP: el config del adapter usa `mason/bin/php-debug-adapter` (corregido — antes lanzaba `php`
> a secas y daba "Adapter didn't respond"). PHP además requiere **Xdebug activo** (puerto 9003)
> y **disparar una request HTTP** (curl/abrir la URL) para que Xdebug conecte al listener.

## 4. Otros paquetes Mason recomendados (roadmap + stack)

Estos complementan los lenguajes del roadmap (Rust, PHP, Go, C, C++, C#, Java) que ya tienen
DAP. Faltan los **LSPs** — sin LSP el autocompletado/diagnóstico no funciona.

### LSPs por lenguaje (priority)

| LSP                     | Lenguaje     | Notas                                   |
| ----------------------- | ------------ | --------------------------------------- |
| `rust-analyzer`         | Rust         | el LSP oficial de Rust                  |
| `gopls`                 | Go           | LSP oficial de Go                       |
| `phpactor` o `intelephense` | PHP      | LSP de PHP; intelephense requiere key   |
| `clangd`                | C / C++      | el LSP estándar de C/C++                |
| `cpptools`              | C / C++      | ✅ **Instalado** (vscode-cpptools) — lo instala `mason-lspconfig`; al compilar descarga su schema `vscode-cpptools/.../package.json` |
| `jdtls`                 | Java         | Java LSP (Eclipse)                      |
| `omnisharp` / `csharp_ls` | C# / .NET  | LSP de C#                               |

### DAPs extra (opcionales)

| DAP            | Lenguaje      | Notas                          |
| -------------- | ------------- | ------------------------------ |
| `bash-debug-adapter` | Bash    | depura scripts bash            |
| `debugpy`      | Python        | si algún día agregas Python    |
| `js-debug-adapter`   | JS/TS | ya cubierto por `pwa-node` (no hace falta) |

### Linters / formatters útiles (ya tienes shfmt, prettier, stylua, eslint, biome)

| Paquete     | Lenguaje      | Notas                       |
| ----------- | ------------- | --------------------------- |
| `shellcheck`| Shell/bash    | lint de scripts shell       |
| `clang-format` | C/C++     | formateador C/C++           |
| `gofmt`     | Go            | formateador Go (viene con Go) |
| `google-java-format` | Java  | formateador de Google Java  |
| `markdownlint` | Markdown  | ya tienes `markdownlint-cli2` |

## 5. Resumen mental

- **Mason = el gestor** de LSP/DAP/linters/formatters (no son plugins de nvim, son binarios).
- `mason.nvim` instala LSPs; `mason-nvim-dap` instala DAPs; ambos con `ensure_installed`.
- El `ensure_installed` de nvim-dap **solo corre al cargar nvim-dap** (lazy), no al abrir nvim
  → pero **ya se disparó**: los 5 DAPs están **Installed** (confirmado en lista Mason).
- Los **LSPs** (p. ej. `cpptools`) los auto-instala **`mason-lspconfig`** por un `ensure_installed`
  distinto: si ves "Installing cpptools / Downloading schema vscode-cpptools" es normal, es ese.
- En Mason: `<C-f>` abre el **Language Filter** — escribís la extensión (ej. `cpp`) y `<Enter>`
  la aplica para ver solo los paquetes de ese lenguaje.
- Mason es **por-máquina**: lo instalado persiste en esa PC, no se replica a otras.
- **Java**: DAP via extra LazyVim `lang.java` (jdtls). Falta `:MasonInstall jdtls` (JDK 21 ya hay).
- Atajo rápido: `Space + c + m` para abrir Mason y ver qué tienes instalado/available.

---
Fuentes: `nvim-dap.lua` · `mason.lua` · Mason docs: <https://github.com/mason-org/mason.nvim>
