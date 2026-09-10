#!/usr/bin/env bash
set -euo pipefail

SRC="/home/diego/workspace/tmp-testing"
NAMES="README.md testcobl testcpp testcs testgo testjava testjs testphp testpython testrust"

for name in $NAMES; do
  target="/tmp/$name"
  if [ -L "$target" ] && [ "$(readlink -f "$target")" = "$SRC/$name" ]; then
    echo "OK   $target"
    continue
  fi
  if [ -e "$target" ] && [ ! -L "$target" ]; then
    echo "SKIP $target (existe y no es un symlink)"
    continue
  fi
  rm -f "$target"
  ln -s "$SRC/$name" "$target"
  echo "LINK $target -> $SRC/$name"
done