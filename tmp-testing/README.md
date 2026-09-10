# Backup /tmp: Archivos Test para aprender a Debugear/Testing [+9 lang]

## ¿Cómo usar?

> En neovim usar `vim-dap-debug` y `vim-ui`. O el navegador.

- En `vim-dap-debug` usar `Space + d + c` para **Continuar** y `Space + d + b` para **Poner en breakpoint**.

> Ubica las carpetas en:

- `/tmp/`
- `/tmp/testcobl/`
- `/tmp/testcpp/`
- `/tmp/testcs/`
- `/tmp/testgo/`
- `/tmp/testjava/`
- `/tmp/testjs/`
- `/tmp/testphp/`
- `/tmp/testpython/`
- `/tmp/testrust/`

### Código

| Nombre     | Descripción                            | Código                          |
| ---------- | -------------------------------------- | ------------------------------- |
| Cobol      | Hello from COBOL (GnuCOBOL + codelldb) | [test.cbl](testcobl/test.cbl)   |
| C++        | Hello from C++ (codelldb)              | [main.cpp](testcpp/main.cpp)    |
| C#         | Hello from C# (netcoredbg)             | [Program.cs](testcs/Program.cs) |
| Go         | Hello from Go (delve)                  | [main.go](testgo/main.go)       |
| Java       | Hello from Java (jdtls)                | [Main.java](testjava/Main.java) |
| Javascript | Hello from Javascript (pwa-node)       | [index.js](testjs/index.js)     |
| PHP        | Hello from PHP (Xdebug)                | [index.php](testphp/index.php)  |
| Python     | Hello from Python (debugpy)            | [main.py](testpython/main.py)   |
| Rust       | Hello from Rust (codelldb)             | [main.rs](testrust)              |

### Ejecución

```sh
# Cobol
cd /tmp/testcobl && cobc -x test.cbl
# C++
cd /tmp/testcpp && g++ -g -O0 main.cpp -o build/main
# C#
cd /tmp/testcs && dotnet build
# Go
cd /tmp/testgo && go build -o build/main main.go
# Java
cd /tmp/testjava && javac -g -d out Main.java
# Javascript
cd /tmp/testjs && npm run dev
# PHP
cd /tmp/testphp && php -d xdebug.mode=debug -d xdebug.start_with_request=yes \
    -d xdebug.client_host=127.0.0.1 -d xdebug.client_port=9003 \
    -S localhost:8000
```

#### Notes: ¿Por qué en tmp? ¿Por qué no en home/workspace/tmp?

- Backup: Lo guare en workspace para que no se pierdan los cambios en `/tmp/` cuando se cierra nvim.
- Why (Because): Tras limpiar el cache /tmp desaparecen [nixconf-cleanup].
- When: Usa estos test para confirmar que nvim-dap funcione.
- How: Opencode no usa `~/workspace/tmp-testing`, usa /tmp porque es más fácil de limpiar.
