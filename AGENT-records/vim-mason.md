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

**Solución aplicada:** instalar manualmente con `:MasonInstall codelldb delve netcoredbg
java-debug-adapter php-debug-adapter` en el neovim actual. Ya quedaron marcados como
**Installed** (los 5 ✓).

## 3. Paquetes DAP instalados (adapters por lenguaje)

| Adapter            | Lenguaje(s)        | Binario en Mason      | Config en nvim-dap.lua |
| ------------------ | ------------------ | --------------------- | ---------------------- |
| `codelldb`         | Rust / C / C++     | `mason/bin/codelldb`  | adapter `server` puerto 13000 |
| `delve`            | Go                 | `dlv`                 | adapter `go` con spawn `dlv dap` |
| `netcoredbg`       | C# / VB / F#       | `mason/bin/netcoredbg`| executable con `--interpreter=vscode` |
| `java-debug-adapter`| Java              | `mason/bin/java-debug-adapter` | executable con jar del paquete |
| `php-debug-adapter`| PHP (Xdebug)       | `mason/bin/php-debug-adapter` | executable, puerto 9003 |
| `pwa-node`         | JS/TS (Node)       | (viene de `vscode-js-debug`) | adapter `server` puerto 53700 |

> PHP requiere **Xdebug activo en php.ini** para que el adapter responda (puerto 9003).

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
- El `ensure_installed` de nvim-dap **solo corre al cargar nvim-dap** (lazy), no al abrir nvim.
- Atajo rápido: `Space + c + m` para abrir Mason y ver qué tienes instalado/available.

---
Fuentes: `nvim-dap.lua` · `mason.lua` · Mason docs: <https://github.com/mason-org/mason.nvim>
