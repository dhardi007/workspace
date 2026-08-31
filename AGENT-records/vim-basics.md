# Vim basics — fundamentos y trucos de comandos `:`

Sesión `vim-learn` · Compilado por el Profesor desde:

- `mi_gdrive/.../- [5] Instalar LazyVim`
  `+{atajos VSCODE} prepack.txt`
- `dotfiles-dizzi/nvim/.config/nvim/docs/`
  `neovim-keymaps.md`
- `dotfiles-dizzi/nvim/.config/nvim/docs/`
  `vim-trainer-spec.md`

> Objectivo: que Diego recupere los trucos de `:`
> y fundamentos que se le olvidaron, agrupados en
> una sola chuleta de referencia. Fundamentos
> obvios (h/j/k/l) no se repiten.

---

## 1. Comandos de línea de comandos `:`

| Comando              | Qué hace                              |
| -------------------- | ------------------------------------- |
| `:q`                 | Cerrar ventana/buffer                 |
| `:q!`                | Forzar cerrar (descarta cambios)      |
| `:w`                 | Guardar                               |
| `:wq!`               | Guardar y salir forzado               |
| `:mes` / `:messages` | Ver mensajes de depuración / errores  |
| `:Mason`             | Actualizar/revisar Mason (LSPs)       |
| `:Lazy sync`         | Actualizar plugins Lazy               |
| `:Lazy reload <plugin>` | Recargar SOLO un plugin sin reiniciar Neovim (útil tras editar su config, ej. `:Lazy reload nvim-dap`) |
| `:10`                | Ir a la línea 10 (dice Nº)            |
| `:WhichKey`          | Ver todos los atajos/keymaps          |
| `:pwd`               | Ver directorio de trabajo actual      |
| `:lcd <ruta>`        | Cambiar cwd solo de la ventana actual |

## 2. Sustitución / reemplazo (`:s`)

### ¿Qué significa `foo` y `bar`?

`foo` y `bar` son **nombres de relleno** (placeholders)
que la gente usa en ejemplos (como "tal cosa" /
"cualquier palabra"). NO son especiales en Vim: solo
representan **palabra vieja** y **palabra nueva**.

```text
: s  /  foo  /  bar  /  g
  │      │       │     └─ g = todas las veces
  │      │       └─ EL TEXTO NUEVO
  │      └─ EL TEXTO VIEJO
  └─ substitute = reemplazar
```

### Desglose de cada parte del comando

| Parte        | Qué significa                      |
| ------------ | ---------------------------------- |
| `:s`         | "substitute" → reemplazar texto    |
| `foo`        | lo que buscás (reemplazar ESTO)    |
| `bar`        | por lo que lo cambiás              |
| `/g`         | `g` = **global** → todas las veces |
| `%` (inicio) | aplica a **todo el archivo**       |
| `c` (final)  | `c` = **confirmar** → te pregunta  |
| `\< \>`      | límites de palabra (no "foobar")   |

### Ejemplo concreto

Imaginá tu archivo con la palabra `AuthUser`
repetida a la mano, y querés cambiarla a `Usuario`:

```vim
:%s/AuthUser/Usuario/g
```

- `%` → todo el archivo
- `s` → reemplazar
- `AuthUser` → buscar eso
- `Usuario` → poner esto en su lugar
- `g` → todas las ocurrencias

Si querés que ANTES de cambiar cada una te pregunte
(seguro), agregá `c`:

```vim
:%s/AuthUser/Usuario/gc
```

### ¿Qué es "trailing whitespace"?

Espacios en **blanco al final de la línea**
("trailing" = que arrastra al final).
Se ven como nada pero ensucian el código:

```text
const x = 1     ← hay 4 espacios al final
```

Para borrarlos de todo el archivo:

```vim
:%s/\s\+$//g
```

- `\s` = un espacio en blanco
- `\+` = uno o más
- `$` = final de línea
- Es decir: "quitar los espacios que estén justo
  antes del final de línea".

### Tabla resumen

| Comando             | Qué hace                              |
| ------------------- | ------------------------------------- |
| `:s/foo/bar/`       | Línea actual, primera ocurrencia      |
| `:s/foo/bar/g`      | Línea actual, todas las veces         |
| `:%s/foo/bar/g`     | Todo el archivo (el más usado)        |
| `:%s/foo/bar/gc`    | Con confirmación antes de cada cambio |
| `:'<,'>s/foo/bar/g` | Solo la selección visual              |
| `:10,20s/foo/bar/g` | Solo líneas 10-20                     |
| `:%s/\s\+$//g`      | Borrar trailing whitespace            |
| `:%s/\<foo\>/bar/g` | Solo palabras completas               |

> 📌 Si querés ver estas explicaciones como chuleta
> de EDICIÓN (motions/flujo de armar el comando con
> `*` o el combo selectivo), están en
> `vim-motions.md`. Acá es solo el "qué hace
> cada comando".

## 3. Combo mágico → ver `vim-motions.md`

El flujo `* + cgn + .` para reemplazar eligiendo
cada caso (y el truco de `* + :%s //`) está
documentado en **`vim-motions.md`** (sección de
edición), porque pertenece a los motions de edición,
no a los comandos `:` de fundamentos.

- `*` → `cgn` → texto → `Esc` → `.` → `n` → `.`
- Está explicado con detalle en `vim-motions.md`.

## 4. `:cfdo` — ⚠️ peligro (lo que te pasó)

`:cfdo s/old/new/g | update` aplica sobre TODAS
las líneas de la quickfix list (de muchos archivos).
Si el patrón no es exacto (o `/` quedó vacío/heredado
de `*`), **puede borrar de más**.

Regla: **no uses `:cfdo` a ciegas**. Para reemplazo
en varios archivos con control, preferí el combo
mágico de arriba (sección 3) o `%s` con
confirmación (`gc`).

## 5. Vimgrep

| Comando                      | Qué hace                    |
| ---------------------------- | --------------------------- |
| `:vimgrep /patrón/g **/*.ts` | Buscar en todos los .ts     |
| `:vimgrep /TODO/g **/*`      | Buscar TODOs en el proyecto |
| `:copen` / `:cclose`         | Abrir/cerrar quickfix list  |
| `:cnext` / `:cprev`          | Ir al siguiente/anterior    |

## 6. Macros

| Comando          | Qué hace                |
| ---------------- | ----------------------- |
| `qa`             | Empezar a grabar en 'a' |
| `q`              | Parar de grabar         |
| `@a`             | Ejecutar el macro 'a'   |
| `@@`             | Repetir último macro    |
| `5@a`            | Ejecutar macro 5 veces  |
| `:5,10normal @a` | Ejecutar en líneas 5-10 |

Tips: empezá con `0`/`^`, terminá con `j`.

## 7. Modo visual (V)

| Comando  | Qué hace                        |
| -------- | ------------------------------- |
| `V`      | Visual line (selecciona líneas) |
| `%`      | Seleccionar todo (`:%` + y/d)   |
| `:%y`    | Copiar todo                     |
| `:%d`    | Cortar todo                     |
| `Ctrl+V` | Visual block                    |

## 8. Registros / yank / paste

| Comando      | Qué hace                    |
| ------------ | --------------------------- |
| `y`          | Copiar (yank)               |
| `yy`         | Copiar línea                |
| `p` / `P`    | Pegar después / antes       |
| `d`          | Cortar/borrar (al registro) |
| `dd`         | Borrar línea                |
| `D`          | Borrar hasta final de línea |
| `u` / Ctrl+R | Deshacer / rehacer          |

## 9. Modo normal — accesos rápidos

| Comando   | Qué hace                       |
| --------- | ------------------------------ |
| `o`       | Insertar línea debajo + insert |
| `Shift+O` | Insertar línea arriba + insert |

> ⚠️ **`O` según el modo:**
>
> - Normal `O`/`Shift+O` = insertar línea en blanco arriba.
> - **Visual `O` = alternar la dirección/ancla de la selección** (salta al otro extremo: fila
>   superior ↔ inferior; en `Ctrl+V` alterna esquinas) — práctico para extender la selección
>   al lado opuesto.
> - Para **mover** físicamente líneas/bloques se usa el plugin `move.nvim` con `Alt+h/j/k/l`
>   (y `F2`/`F3`) → ver atajos en `vim-motions.md`.
>   | `[` / `]` | Inicio / final (según contexto) |
>   | `[b` / `]b` | Tab anterior / siguiente |
>   | `gg` | Primera línea |
>   | `Shift+G` (`G`) | Última línea |
>   | `Space+qq` | Cerrar Neovim rápido |

## 10. Indentar en visual

| Comando                      | Qué hace           |
| ---------------------------- | ------------------ |
| `V + <` / `V + >`            | Indentar izq / der |
| `V + Ctrl+Space+Space+Space` | Seleccionar TODO   |

## 11. Linux / clipboard en Nvim

```bash
# Ver si estás en X11 o Wayland
echo $XDG_SESSION_TYPE
# Si es wayland:
sudo pacman -S wl-clipboard
# Si es x11:
sudo pacman -S xclip xsel
```

Para que y/p usen el portapapeles del sistema
(en `init.lua`):

```lua
vim.opt.clipboard:append("unnamedplus")
```

## 12. rg + fzf (fuera de nvim)

```bash
yay -Qe | rg espanso
rg "font" ~/.config/ghostty/config
yay -Qe | fzf
nvim $(fzf)
```

---

## Referencias originales

- Prepack LazyVim:
  `mi_gdrive/Mi unidad/- Linux Hyprland [Arch]/`
  `- [5] Instalar LazyVim +{atajos VSCODE}`
  `prepack.txt`
- Keymaps:
  `dotfiles-dizzi/nvim/.config/nvim/docs/`
  `neovim-keymaps.md`
- Trainer:
  `dotfiles-dizzi/nvim/.config/nvim/docs/`
  `vim-trainer-spec.md`
