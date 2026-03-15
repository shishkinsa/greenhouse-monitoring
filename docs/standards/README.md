# Стандарты и соглашения

В этом разделе собраны единые правила именования, стиля кода и работы с Git. Для C# часть правил проверяется в [src/.editorconfig](../src/.editorconfig); для TypeScript, React и SQL в .editorconfig задано только форматирование (отступы, перевод строки), именование описано в соответствующих документах ниже.

---

## Структура каталога

| Файл | Назначение |
|------|-------------|
| `c-charp-naming-conventions.md` | Именование в C# (типы, интерфейсы, методы, поля и т.д.) |
| `ts-naming-conventions.md` | Именование в TypeScript (файлы, типы, переменные, функции) |
| `react-naming-conventions.md` | Именование в React (компоненты, props, хуки, обработчики) |
| `psql-naming-conventions.md` | Именование в PostgreSQL (таблицы, колонки, индексы, представления) |
| `coding-standards.md` | Общие принципы кодирования (KISS, DRY, по языкам, тесты, инструменты) |
| `git-flow.md` | Процесс работы с Git (ветки, коммиты, PR, релизы) |

---

## Документы

| Документ | Содержание |
|----------|------------|
| [c-charp-naming-conventions.md](c-charp-naming-conventions.md) | Соглашения по именованию C#: пространства имён, классы, интерфейсы (I + PascalCase), методы, свойства, поля (_ + camelCase), константы, enum, DTO. Проверка через .editorconfig. |
| [ts-naming-conventions.md](ts-naming-conventions.md) | Соглашения по именованию TypeScript: файлы и папки kebab-case, интерфейсы/типы/классы PascalCase (без префикса I), переменные и функции camelCase, константы UPPER_SNAKE_CASE, generics (T, K, V). |
| [react-naming-conventions.md](react-naming-conventions.md) | Соглашения по именованию React: компоненты PascalCase, файлы компонентов .tsx, props-интерфейсы (ComponentName + Props), хуки use*, обработчики handle*, колбэки в пропсах on*, useState/useEffect, условный рендеринг. |
| [psql-naming-conventions.md](psql-naming-conventions.md) | Соглашения по именованию PostgreSQL: БД, схемы, таблицы и колонки snake_case; индексы idx_*, внешние ключи fk_*, последовательности *_seq, представления v_*, функции/процедуры, триггеры tr_*. |
| [coding-standards.md](coding-standards.md) | Общие стандарты кодирования: KISS, DRY, принципы по C#, TypeScript, React, SQL, тестирование, документирование, безопасность, производительность, инструменты. |
| [git-flow.md](git-flow.md) | Процесс работы с Git: структура веток (main, develop, feature, release, hotfix), именование веток, коммиты, Pull Request, релизный процесс и hotfix, команды и чек-листы. |
