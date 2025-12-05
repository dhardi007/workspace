#!/bin/bash
echo "🚀 Configurando workspace..."

# Verificar submodules
git submodule update --init --recursive

# Crear enlaces simbólicos útiles
ln -sf configs/.mcp-global.json .mcp-global.json 2>/dev/null || true

echo "✅ Workspace listo!"
