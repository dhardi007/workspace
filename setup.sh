#!/bin/bash

# Definir colores
ROJO=$(tput setaf 1)
VERDE=$(tput setaf 2)
AMARILLO=$(tput setaf 3)
AZUL=$(tput setaf 4)
MAGENTA=$(tput setaf 5)
CYAN=$(tput setaf 6)
BLANCO=$(tput setaf 7)
RESET=$(tput sgr0)

echo "${VERDE}🚀 Configurando workspace...${RESET}"
echo ""

echo "${AZUL}Paso 1: Clonando repositorios...${RESET}"
# Verificar submodules
git submodule update --init --recursive
rm -rf Librezam retro-portfolio kimu-underground portfolio-terminal-dhardi GLAZE-WM-make-windows-pretty-main-dizzi

# Recuperar cada submódulo
git submodule update --init --recursive Librezam
git submodule update --init --recursive retro-portfolio
git submodule update --init --recursive GLAZE-WM-make-windows-pretty-main-dizzi
git submodule update --init --recursive kimu-underground
git submodule update --init --recursive portfolio-terminal-dhardi
git submodule update --init --recursive PCE-Agencia

echo ""
echo "${AMARILLO}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${RESET}"
echo "${ROJO}Paso 2 [TEORÍA]: BORRAR PERMANENTEMENTE los submódulos...${RESET}"
echo "${AMARILLO}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${RESET}"
echo ""
echo "${CYAN}Script completo para eliminarlos definitivamente:${RESET}"
echo "  ${MAGENTA}git submodule deinit -f Librezam${RESET}"
echo "  ${MAGENTA}git rm -f Librezam${RESET}"
echo "  ${MAGENTA}rm -rf .git/modules/Librezam${RESET}"
echo "  ${CYAN}Lo mismo con el resto...${RESET}"
echo ""

echo "${AMARILLO}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${RESET}"
echo "${ROJO}Paso 3 [TEORÍA]: Agregar un nuevo submodule...${RESET}"
echo "${AMARILLO}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${RESET}"
echo ""
echo "  ${MAGENTA}git submodule add https://el-repo-en-cuestion${RESET}"
echo ""

echo "${AMARILLO}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${RESET}"
echo "${AZUL}Paso 4: Corrigiendo el branch main...${RESET}"
echo "${AMARILLO}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${RESET}"
echo ""

cd ./Librezam/ && git checkout main
cd ../GLAZE-WM-make-windows-pretty-main-dizzi/ && git checkout main
cd ../retro-portfolio/ && git checkout main
cd ../kimu-underground/ && git checkout main
cd ../portfolio-terminal-dhardi/ && git checkout main
cd ../FCTicService.github.6c-Diego-05/ && git checkout main
cd ../REACT-Diego-Dizzi-Dashboard/ && git checkout main
cd ../Proyecto-App-MCSD/ && git checkout main
cd ../PCE-Agencia/ && git checkout main

echo ""
echo "${VERDE}✅ Workspace listo!${RESET}"
