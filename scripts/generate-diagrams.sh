#!/usr/bin/env bash
# Генерация PNG из LikeC4: для каждой категории в docs/architecture/diagram/*
# результат — docs/public/likec4/<категория>/ (см. docs/architecture/diagram/README.md).
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
DIAGRAM_ROOT="${ROOT}/docs/architecture/diagram"
OUT_ROOT="${ROOT}/docs/public/likec4"

if [[ ! -d "${DIAGRAM_ROOT}" ]]; then
  echo "error: missing diagram root: ${DIAGRAM_ROOT}" >&2
  exit 1
fi

mkdir -p "${OUT_ROOT}"

exported=0
while IFS= read -r -d '' dir; do
  if [[ -n "$(find "${dir}" -name '*.c4' -print -quit)" ]]; then
    name="$(basename "${dir}")"
    out="${OUT_ROOT}/${name}"
    mkdir -p "${out}"
    echo "==> likec4: ${dir} -> ${out}"
    (cd "${ROOT}" && npx likec4 export png -o "${out}" "${dir}")
    exported=$((exported + 1))
  fi
done < <(find "${DIAGRAM_ROOT}" -mindepth 1 -maxdepth 1 -type d -print0)

if [[ "${exported}" -eq 0 ]]; then
  echo "warning: no subfolders with .c4 under ${DIAGRAM_ROOT}" >&2
fi

echo "done: ${exported} LikeC4 workspace(s) exported to ${OUT_ROOT}"
