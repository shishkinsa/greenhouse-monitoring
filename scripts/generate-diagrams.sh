#!/bin/bash
# ======================================================
# Простой скрипт для генерации PNG из диаграмм
# Поддерживает: PlantUML, Draw.io
# ======================================================

set -e  # Выход при любой ошибке

# ======================================================
# Цвета для вывода
# ======================================================
GREEN='\033[0;32m'
BLUE='\033[0;34m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m'

# ======================================================
# Конфигурация
# ======================================================
SOURCE_DIR="docs/architecture/diagram"
OUTPUT_DIR="docs/public"

# ======================================================
# Функции
# ======================================================
print_success() {
    echo -e "${GREEN}✅ $1${NC}"
}

print_error() {
    echo -e "${RED}❌ $1${NC}"
}

print_info() {
    echo -e "${BLUE}ℹ️  $1${NC}"
}

print_warning() {
    echo -e "${YELLOW}⚠️  $1${NC}"
}

# ======================================================
# Очистка предыдущих результатов
# ======================================================
print_info "Очистка папки $OUTPUT_DIR..."
rm -rf "$OUTPUT_DIR"
mkdir -p "$OUTPUT_DIR"/{plantuml,drawio}

# ======================================================
# Генерация PlantUML диаграмм
# ======================================================
print_info "Генерация PlantUML диаграмм..."

if command -v plantuml &> /dev/null; then
    # Находим все .puml файлы
    while IFS= read -r file; do
        if [ -n "$file" ]; then
            # Получаем относительный путь от SOURCE_DIR
            rel_path="${file#$SOURCE_DIR/}"
            # Определяем выходную папку (сохраняем структуру)
            out_dir="$OUTPUT_DIR/plantuml/$(dirname "$rel_path")"
            mkdir -p "$out_dir"
            
            print_info "  Обработка: $rel_path"
            plantuml -tpng -charset UTF-8 -o "$(pwd)/$out_dir" "$file"
            print_success "    → $out_dir/$(basename "${file%.puml}.png")"
        fi
    done < <(find "$SOURCE_DIR" -name "*.puml" -type f)
else
    print_warning "PlantUML не установлен. Пропускаем."
fi

# ======================================================
# Генерация Draw.io диаграмм
# ======================================================
print_info "Генерация Draw.io диаграмм..."

# Проверяем наличие drawio CLI
DRAWIO_CMD=""
if command -v drawio &> /dev/null; then
    DRAWIO_CMD="drawio"
elif command -v drawio-desktop &> /dev/null; then
    DRAWIO_CMD="drawio-desktop"
fi

if [ -n "$DRAWIO_CMD" ]; then
    # Находим все .drawio файлы
    while IFS= read -r file; do
        if [ -n "$file" ]; then
            # Получаем относительный путь от SOURCE_DIR
            rel_path="${file#$SOURCE_DIR/}"
            # Определяем выходную папку (сохраняем структуру)
            out_dir="$OUTPUT_DIR/drawio/$(dirname "$rel_path")"
            mkdir -p "$out_dir"
            
            print_info "  Обработка: $rel_path"
            $DRAWIO_CMD --export --format png --output "$out_dir" "$file"
            print_success "    → $out_dir/$(basename "${file%.drawio}.png")"
        fi
    done < <(find "$SOURCE_DIR" -name "*.drawio" -type f)
else
    print_warning "Draw.io CLI не установлен. Пропускаем."
    print_info "Установка: brew install drawio (macOS) или скачайте с draw.io"
fi

# ======================================================
# Итог
# ======================================================
echo ""
print_success "Генерация завершена!"
print_info "PNG файлы сохранены в: $OUTPUT_DIR"
print_info "Структура папок сохранена:"
find "$OUTPUT_DIR" -type f -name "*.png" | sed "s|^$OUTPUT_DIR/|  📁 |" | sort