# Шаблон промта (скопируйте и доработайте блоки в угловых скобках)

```text
Ты — архитектор. Нужно описать целевое развёртывание ИС **Greenhouse Monitoring** в LikeC4: `deployment { environment production 'Production' { … } }`, согласованное с `docs/architecture/01-calc-architecture.md` и контейнерной моделью `greenhouse_system`.

Уже принятая база (сохраняй, если задача — точечное изменение, а не новая система):
- Периметр: L7 `edge` (Nginx, 443/TLS) и отдельно `edge_mqtt` (8883/TLS → RabbitMQ MQTT); контроллеры не ходят в кластер по HTTP Ingress.
- Kubernetes: приложение (веб+API, SavingService, go2rtc live/history, Identity, MinIO, Vault), RabbitMQ 2 узла, Redis 1+1 и Sentinel 3.
- ВМ: PostgreSQL метаданных 1+1, отдельная БД Identity, ClickHouse 2 + Keeper 3.
- Контейнеры только через `instanceOf greenhouse_system.<cnt_*>` — список в `docs/architecture/diagram/containers/`.

Контекст изменения / фокус:
- <что именно меняем: новая зона, другие реплики, другой периметр, новый компонент, DR и т.д.>

Требования к результату:
- Явно опиши три класса трафика (браузер L7, MQTT с поля, RTSP с камер).
- Укажи технологии узлов и число реплик/кворум там, где это важно для HA (NFR-03).
- Не перегружай рёбрами все внутренние связи приложения — резюме портов в `description` среды или зон, как в текущем `30-production-deployment.c4`.
- Если добавляешь вид: `views { deployment view … }` с `include production.**`, при необходимости `exclude` логических рёбер из модели контейнеров (см. `infrastructure/31-views.c4`).

Выход:
1) Фрагмент или полный файл `.c4` с `deployment { … }`.
2) При необходимости — правки к `31-views.c4`.
3) Список допущений и открытых вопросов.
```
