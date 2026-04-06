# Пример промта: dynamic view LikeC4 (аутентификация OIDC)

Используйте текст ниже как основу для чата с ИИ или чек-лист при ручном создании **dynamic view** сценария **входа пользователя** в LikeC4. Подставьте пути к файлам при необходимости.

Ориентир по проекту: [oidc-user-authentication.c4](oidc-user-authentication.c4).

---

## Промт (скопировать)

```text
Ты помощник по архитектурным диаграммам LikeC4 для проекта Greenhouse Monitoring.

Цель: сгенерировать один блок `views { ... }` с **одним dynamic view**, описывающим процесс **аутентификации пользователя** (вход в систему и получение доступа к API ИС), без изменения смысла уже принятой контейнерной архитектуры и без выдумывания новых сервисов.

### Источники правды (прочитай и опирайся на них)
- Акторы и система: docs/architecture/diagram/context/model.c4 (employee, engineer, greenhouse_system).
- Контейнеры Identity и приложения ИС: docs/architecture/diagram/containers/cnt_gm_web/model.c4, cnt_gm_identity_web/model.c4, cnt_gm_identity_webapi/model.c4, cnt_gm_identity_db/model.c4, cnt_gm_webapi/model.c4 (связи OIDC, JWKS, Bearer).
- Спецификация элементов/связей: docs/architecture/diagram/context/specification.c4.
- При необходимости уточнения формулировок безопасности: ADR и docs по Vault/OIDC в docs/architecture/ (если есть в репозитории).

### Правила LikeC4 для dynamic view
- Документация: https://likec4.dev/dsl/views/dynamic
- В шагах используй только элементы, которые **уже объявлены в model** (например `greenhouse_system.cnt_gm_web`, `greenhouse_system.cnt_gm_identity_web`, `greenhouse_system.cnt_gm_identity_webapi`, `greenhouse_system.cnt_gm_identity_db`, `greenhouse_system.cnt_gm_webapi` — согласуй с существующими model.c4). Не придумывай новые контейнеры без явного запроса.
- Имя представления: латиница и snake_case, например `user_authentication_oidc` или `dynamic_security_oidc_login`.
- Для dynamic view укажи `title` и при необходимости `description` на русском.
- Опционально: `variant sequence` для классической диаграммы последовательности; учти, что для sequence может понадобиться явный порядок участников (`include` в представлении — см. доку LikeC4). Если не уверен — используй вариант по умолчанию (diagram).
- Где уместно, добавь `notes` в шагах (поддерживается Markdown): OIDC **Authorization Code + PKCE**, эндпоинты authorize/token, проверка JWT (**JWKS**, issuer, audience), **OpenIddict**, **PostgreSQL** для Identity DB.
- Параллельные обращения оформляй через `parallel { ... }` или `par { ... }` — без вложенных parallel.
- Обратные ответы: `A <- B '...'` — см. доку LikeC4.

### Сценарий — Аутентификация (единый для ролей)
Пользователь может быть **сотрудником** или **инженером**; поток OIDC одинаковый — на диаграмме допустим один представитель (например actor **employee**) с пояснением в `notes` или `description`.

Опиши последовательность (логика, согласованная с моделью):
1. Открытие SPA (**CNT_GM_Web**) и переход к защищённому входу.
2. Редирект на **CNT_GM_Identity_Web** (OIDC `/authorize`, PKCE: code_challenge и т.д.).
3. Взаимодействие Identity Web с **CNT_GM_Identity_WebAPI** (OpenIddict): discovery/authorize, форма входа, проверка учётных данных.
4. Обращение Identity WebAPI к **CNT_GM_Identity_DB** (пользователи, клиенты OIDC, refresh-токены по архитектуре).
5. Возврат в SPA с **authorization code**; обмен code на **access_token** на token endpoint (PKCE **code_verifier**).
6. Дальнейший вызов **CNT_GM_WebAPI** с **Authorization: Bearer** и проверка access token через Identity WebAPI (**JWKS** / metadata по модели).

Не смешивай в этом же представлении бизнес-сценарии (live-видео, поиск событий) — только цепочка входа и валидации токена для доступа к API ИС.

### Вывод
Верни **один фрагмент на языке LikeC4**: блок `views { ... }` с одним `dynamic view`, без дублирования всего `model { }` (предполагается, что модель уже подключена в сборке LikeC4 вместе с этим файлом). В начале файла добавь комментарии трассировки, например:
// Тип: LikeC4 dynamic view (security)
// Тема: аутентификация пользователя (OIDC Authorization Code + PKCE, OpenIddict)

Если какого-то элемента не хватает в текущей model, явно перечисли, что нужно добавить в model сначала, вместо выдумывания имён контейнеров.
```

---

## Как использовать

1. Скопируйте блок внутри ```text … ``` в чат с ИИ или приложите перечисленные `model.c4` как контекст.
2. Сохраните результат в файл в каталоге `docs/architecture/diagram/security/`, например `oidc-user-authentication.c4`. Для разрешения ссылок на акторы и контейнеры настройте в этом каталоге `likec4.config.json` с `include.paths` на `../context` и `../containers` (и при необходимости `exclude` для `**/views.c4`, чтобы не дублировать статические представления), либо включите новый файл в уже существующий workspace диаграмм проекта.
3. Проверьте визуализацию в LikeC4 CLI или IDE; при ошибках «элемент не найден» — проверьте квалификаторы (`greenhouse_system.*`) и наличие акторов в контекстной модели.

## Ссылки

- [Dynamic views — LikeC4](https://likec4.dev/dsl/views/dynamic)
- [Плейграунд (dynamic)](https://playground.likec4.dev/w/dynamic/)
