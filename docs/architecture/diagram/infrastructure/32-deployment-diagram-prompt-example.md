# Пример промта для диаграммы развёртывания

Используйте шаблон ниже в чате с ИИ (или в Cursor), когда нужно сгенерировать или обновить **deployment**-диаграмму **ИС Greenhouse Monitoring** в формате **LikeC4** (`deployment { … }`), согласованную с расчётом нагрузки, контейнерной моделью и текущим эталоном `30-production-deployment.c4`.

## Контекст архитектуры (кратко)

- **Система:** мониторинг теплиц — веб-SPA и API, OIDC (OpenIddict), live/архивное видео (go2rtc), телеметрия с контроллеров по **MQTT** в **RabbitMQ** (внутренние потребители — по **AMQP**), временные ряды в **ClickHouse**, метаданные в **PostgreSQL**, кэш — **Redis** с **Sentinel**, секреты — **HashiCorp Vault**, архив видео — **MinIO (S3)**.
- **Логическая модель LikeC4:** система `greenhouse_system` («Greenhouse Monitoring»), контейнеры задаются в `docs/architecture/diagram/containers/**/01-model.c4` и расширяют `greenhouse_system`.
- **Масштаб и NFR** зафиксированы в `docs/architecture/01-calc-architecture.md`: ~1000 сотрудников и ~50 инженеров (US-01, US-02, NFR-04), 1000+ теплиц, MQTT ≈1000 постоянных соединений, HA по **NFR-03**; вторые узлы там, где нужна отказоустойчивость, а не только запас по производительности.

## Эталонная топология production (не выдумывать заново без причины)

Ориентир — `docs/architecture/diagram/infrastructure/30-production-deployment.c4`:

| Слой | Содержимое (как в эталоне) |
|------|----------------------------|
| Периметр L7 | `edge` — Nginx, TLS 443: SPA, API, OIDC, WSS к go2rtc |
| Периметр MQTT | `edge_mqtt` — Ingress TCP, типично **8883/TLS** → RabbitMQ (MQTT-плагин), **не** через HTTP Ingress |
| Kubernetes | Зона `app`: веб+API (2 реплики), SavingService (2), go2rtc live/history (по 2), Identity SPA+API (2), MinIO distributed (4), Vault Raft (3). Зона `messaging`: RabbitMQ кластер **2** узла. Зона `redis_ha`: Redis **1+1**, Sentinel **3** |
| ВМ в приватной сети | PostgreSQL метаданных **1+1** (репликация), отдельная ВМ **Identity DB** (PostgreSQL), ClickHouse **2** узла + **ClickHouse Keeper 3** (кворум на отдельных узлах) |

**Классы трафика** (на диаграмме их нужно явно разделять текстом и/или отдельными LB):

1. Браузер → `edge` **443/TLS** — SPA, API, OIDC, WSS к go2rtc.
2. Контроллеры теплиц → `edge_mqtt` **8883/TCP** (TLS/MQTT) → RabbitMQ — **не** смешивать с пользовательским L7.
3. IP-камеры → поды go2rtc по **RTSP/RTP** из сегмента/VLAN камер; пользовательский периметр сюда не относится.

Внутренние порты и сводка реплик удобно держать в `description` среды **Production** (как в эталоне): Redis 6379, PostgreSQL 5432, RabbitMQ AMQP 5672, ClickHouse HTTP 8123/8443, Vault HTTPS (~8200), MinIO S3, Keeper **9181/TCP**, Sentinel **26379/TCP**.

## Идентификаторы контейнеров для `instanceOf`

Ссылайтесь только на существующие элементы модели: `greenhouse_system.<контейнер>`.

| Контейнер | Назначение |
|-----------|------------|
| `cnt_gm_web` | SPA основного приложения |
| `cnt_gm_webapi` | GraphQL/Web API |
| `cnt_gm_savingservice` | Потребитель AMQP (ingestion телеметрии) |
| `cnt_gm_webrtcserver_only` | go2rtc live |
| `cnt_gm_webrtcserver_history` | go2rtc архив |
| `cnt_gm_identity_web` | SPA Identity |
| `cnt_gm_identity_webapi` | API Identity / OpenIddict |
| `cnt_gm_s3` | MinIO S3 |
| `cnt_gm_secrets` | Vault |
| `cnt_gm_broker` | RabbitMQ |
| `cnt_gm_redis_db` | Redis |
| `cnt_gm_db` | PostgreSQL (метаданные приложения) |
| `cnt_gm_identity_db` | PostgreSQL Identity |
| `cnt_gm_timeseries_db` | ClickHouse |

## Входные материалы для промта

- Расчёт и обоснование узлов: `docs/architecture/01-calc-architecture.md`
- Контейнеры и протоколы: `docs/architecture/diagram/containers/`
- Эталон развёртывания: `docs/architecture/diagram/infrastructure/30-production-deployment.c4`
- Вид диаграммы (include/exclude, заголовок): `docs/architecture/diagram/infrastructure/31-views.c4`
- Поведение и OIDC при необходимости уточнений: `docs/architecture/diagram/behavior/`, `docs/architecture/diagram/security/`

## Шаблон промта (скопируйте и доработайте блоки в угловых скобках)

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

## Подсказки по стилю репозитория

- Заголовок вида и пояснение «что на схеме и что в тексте» — в `31-views.c4` (`deployment view production_deployment`).
- На deployment-часто **исключают** дублирующие логические связи OIDC/Web→WebRTC из контейнерной модели, оставляя периметр и узлы.
- Связи периметра к подам оформляйте с `technology`: например `443/TLS`, `8883/TCP via TLS`.
- После правок прогоните сборку LikeC4 и обновите снапшоты, если они используются в CI.

## Связанные файлы

| Путь | Назначение |
|------|------------|
| `30-production-deployment.c4` | Модель развёртывания production |
| `31-views.c4` | Вид `production_deployment`, include/exclude |
| `docs/architecture/diagram/containers/` | Контейнеры `greenhouse_system.*` |
| `docs/architecture/01-calc-architecture.md` | RPS/CCU, брокер, HA, объёмы хранения |
