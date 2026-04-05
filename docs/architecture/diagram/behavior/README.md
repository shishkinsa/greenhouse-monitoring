# Диаграммы поведения (behavior)

Каталог для **сценарных** представлений: последовательности взаимодействий во времени, в том числе **dynamic views** LikeC4.

- Документация LikeC4: [Dynamic views](https://likec4.dev/dsl/views/dynamic) (шаги `A -> B`, `parallel { }`, вариант `sequence`, `notes`, `navigateTo`).
- Статические контейнеры и акторы задаются в `docs/architecture/diagram/context/` и `docs/architecture/diagram/containers/`; dynamic view описывает **конкретный сценарий** без дублирования полной модели в каждом шаге (связи ссылаются на уже объявленные элементы).

Рекомендуемые имена файлов по [README родительского каталога](../README.md): диапазон **40–49** для поведения, например `41-employee-live-monitoring.c4`.

Пример **текста промта** для ИИ или автора диаграмм: [prompt-dynamic-views.example.md](prompt-dynamic-views.example.md).
