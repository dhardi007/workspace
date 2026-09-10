# HK / Silksong + Bottles — Fixes e intentos (2026-09-10)

Contexto: embudo Discord Rich Presence + lanzamiento de Hollow Knight y Silksong desde
la botella `gaming` de Bottles (Flatpak) en NixOS. El objetivo: que los juegos corran
con el entorno de la botella (mods BepInEx + Discord bridge) y no con el `~/.wine` del
host, que no tiene los mods.

## Rutas clave

- Botella: `/home/diego/.var/app/com.usebottles.bottles/data/bottles/bottles/gaming`
- `bottle.yml`: `Runner: wine-ge-proton8-26`, `DXVK: dxvk-3.0.2`, `VKD3D: vkd3d-proton-3.0.1`,
  `dependencies: mono, gecko, d3dx9, d3dcompiler_43/47, msls31, arial32, times32, courie32`
  → NO hay .NET Framework (`microsoft.net` ausente), solo mono.
- Runners en disco:
  - `ge-proton11-3` = custom tool Proton (tiene `proton/`, `protonfixes/`, `compatibilitytool.vdf`)
    → es **GE-Proton**, no un runner de Wine clásico.
  - `wine-ge-proton8-26` = runner Wine-GE real (`bin/wine`, `bin/wine64`, `lib64/wine/`, `lib/wine/`).
- Juegos: `.../bottles/gaming/drive_c/Games/Hollow Knight Silksong/Hollow Knight Silksong.exe`
  y `Hollow Knight/Hollow Knight.exe`.
- Mods confirmados (con `ls` real del usuario; un `find` con `2>/dev/null` mintió):
  - Silksong: `BepInEx/plugins/{Silksong_Rich_Presence.dll, SilksongDeathCounter.dll, discord_game_sdk.*}` + `BepInEx/{config,core}`.
  - HK: `Plugins/HollowKnightDRPC.dll` + `x86/` + `x86_64/` + `ReadMe.md`.

## Bridges Discord — hechos verificados

1. **`engineered/rpc-bridge`** (enderice2), binario `bridge.exe` **x86-64**:
   - Registra named pipe `\\.\pipe\discord-ipc-0` → socket host `/run/user/1000/discord-ipc-0`.
   - Corre en `~/.wine` como servicio de Windows (auto) y en la botella tras `bridge.exe → Install`.
   - Es el que funciona en la botella.
2. **`wine-discord-ipc-bridge`** (0e4ef622), binario `winediscordipcbridge.exe` **i386 (32-bit)**:
   - Usa el MISMO pipe `\\.\pipe\discord-ipc-0` → **excluyente** con rpc-bridge en un mismo prefijo.
   - En `~/.wine`: `CreateNamedPipe failed GLE=231` = ERROR_PIPE_BUSY cuando rpc-bridge ya tiene el pipe.
   - En botella: `failed to start winediscordipcbridge.exe: c0000135` (STATUS_DLL_NOT_FOUND).
     **NO es falta de .NET** (la botella tiene mono + `mscoree.dll` en system32 Y syswow64);
     es porque el exe es i386 y el bridge de la botella es x86-64.
3. **Pipe compartido**: ambos bridges usan `\\.\pipe\discord-ipc-0` → regla: **1 pipe por prefijo,
   nunca los dos a la vez**. Documentado en README.md L406+.
4. `~/.wine` NO tiene `syswow64/mscoree.dll` → errores `mscoree/mscorsvw/clr_optimization` en
   logs de wine son ruido de wineboot, inofensivos.

## Causa del "no abren desde Bottles"

Los `.desktop` usan `Exec=wine "<exe en drive_c>"` → wine del HOST + prefijo `~/.wine`
(Sin runner de la botella: sin DXVK/vkd3d/GE/mono). Silksong es Unity/D3D12 y el wine host
no lo renderiza bien. Fix pendiente (Fase 1): `.desktop` debe lanzar con el runner de la
botella + `WINEPREFIX` de la botella, nunca `wine` del host.

## Fase 0 — Intentos de lanzamiento (logs)

| # | Comando | Resultado |
|---|---|---|
| 0.1 | `flatpak run --command=bottles-cli com.usebottles.bottles run -b gaming -e "C:\Games\Hollow Knight Silksong\Hollow Knight Silksong.exe"` | `err:vulkan:wine_vkCreateInstance Failed to create instance, res=-9` (VK_ERROR_INCOMPATIBLE_DRIVER) → DXVK no encuentra driver NVIDIA Vulkan dentro del sandbox. Log: `/tmp/hk-silksong-bottles.log` |
| 0.2 | `wine-ge-proton8-26/bin/wine` (ELF i386) | `No such file or directory` → host NixOS sin multilib 32-bit (`/lib/ld-linux.so.2` no existe) |
| 0.3 | `wine-ge-proton8-26/bin/wine64` (ELF x86-64) | `could not load ntdll.so: libunwind.so.8` → falta lib en host. |
| 0.4 | `wine64` + `LD_LIBRARY_PATH=/nix/store/...-libunwind-1.8.3/lib` | Avanza: **Unity carga** (memorysetup...), pero `Wine cannot find libfreetype` → falta `libfreetype.so.6`. Log: `/tmp/silksong-wine64.log` |

Sandbox Vulkan visto vía `vulkaninfo` dentro del flatpak: Intel UHD 630, NVIDIA GTX 1650
como **NVK (nouveau)**, llvmpipe. El loader OK (vulkaninfo funciona) pero DXVK de wine
recibe INCOMPATIBLE_DRIVER. El host tampoco usa driver NVIDIA propietario: NO hay
`nvidia-smi`, módulo `nouveau` cargado, y las únicas extensiones GL de flatpak son
`org.freedesktop.Platform.GL.default` (Mesa 26.1.5/26.1.6).

## Fase 0 — ÉXITO (confirmado por Diego, fecha 2026-09-10)

**HK y Silksong YA ARRANCAN.** Log del usuario (terminal):
`fixme:d3d11:d3d11_device_CheckFeatureSupport`, `fixme:dxgi`, `GetFrameStatistics`,
`AvSetMmThreadCharacteristics` → ruido inofensivo de wined3d; el juego corre.

Causa raíz confirmada: faltaban las libs del host en el runtime de wine:
- `libunwind.so.8` → `/nix/store/as9c067rglzrz46xs1c1kpqbpj2x77r1-libunwind-1.8.3/lib`
- `libfreetype.so.6` → `/nix/store/09gac8c1amws12ysg2d8lbhd6ya0rwwa-freetype-2.14.3/lib`
- `libXft.so.2` → `/nix/store/55cyrj2wpgci3sxfp8055ff12gcjyb36-libxft-2.3.9/lib/libXft.so.2`
  (¡ojo! el archivo es **`libXft`** con X mayúscula — un find con `libxft.so*` falla.)

Patch aplicado → `/home/diego/dotfiles-dizzi/home/install-bottles.sh` **PASO 2.1** "Exportar libs
de Wine (NixOS)": resuelve las 3 libs en `/nix/store` (system current + fallback find) y exporta
`LD_LIBRARY_PATH` antes de PASO 2.5 (prefix) y PASO 6 (deps). Inofensivo en Arch (salta con warning).

Fixes auxiliares confirmados OK: `.desktop` ya lanzan por la botella (Fase 1.1 ✓); cambiar
internamente el runner en Bottles también ayudó (hipótesis del usuario).
El mensaje `/lib/ld-linux.so.2: could not open` (un helper i386) + `wine: RLIMIT_NICE <=20` son
ruido esperado en NixOS sin multilib — no bloquea (el juego es x86-64).

## Fase 0.3/0.4 — CONFIRMADO (user)

- Mods BepInEx/DRPC **cargan correctamente** con el lanzamiento por botella (confirmado por Diego).
- dotnet48 **no hizo falta** (los mods ya funcionan con el setup actual).

## Pendientes (siguiente sesión)

- NADA pendiente: Fase 1.2 descartada (innecesaria, confirmado por user) y Fase 1.3 completada
  (README: sub-sección "Lanzar juegos por la botella (NUNCA wine del host)" + nota libs NixOS).

## Reglas de seguridad (no romper la botella)

1. NO cambiar el runner de `gaming` a GE-Proton/custom tool (rompe BepInEx/.NET).
2. NO recrear el prefix (borraría mods + bridge).
3. NO reinstalar dotnet48 por encima con winetricks (duplicados/arch corrupta).
4. Lanzar siempre con el runner del prefix + `WINEPREFIX` de la botella, nunca `wine` host.