# Технологический стек (по архитектуре)

Свод основан на моделях LikeC4 в `docs/architecture/diagram/containers/` и `docs/architecture/diagram/context/`. При смене стека сначала обновляют архитектурные модели и требования, затем этот файл.

**Детальные решения и альтернативы** зафиксированы в ADR: [.NET / ASP.NET Core](../architecture/adr/0001-dotnet-aspnet-core-backend.md), [React и TypeScript](../architecture/adr/0002-react-typescript-frontend.md), [PostgreSQL](../architecture/adr/0003-use-postgres.md), [ClickHouse](../architecture/adr/0004-clickhouse-telemetry.md), [RabbitMQ с MQTT](../architecture/adr/0005-rabbitmq-mqtt-broker.md), [MinIO](../architecture/adr/0006-minio-s3-object-storage.md), [HashiCorp Vault](../architecture/adr/0007-hashicorp-vault-secrets.md), [Redis и Sentinel](../architecture/adr/0008-redis-sentinel-cache-sessions.md).

## Пояснение по выбору технологий (сводка)

Ниже — сжатое обоснование в терминах архитектуры и расчётов нагрузки ([calc_architecture.md](../architecture/calc_architecture.md)); в таблицах — колонка «Почему выбрана» с отсылками к ADR, где они есть.

- **.NET / ASP.NET Core и OpenIddict** — единый серверный стек для API, ingestion и Identity; OIDC согласован с клиентом ([ADR-0001](../architecture/adr/0001-dotnet-aspnet-core-backend.md)).
- **React и TypeScript** — типизированный SPA для мониторинга и интеграции с GraphQL/WebSocket; React 18.2.0 зафиксирован в метаданных Web ([ADR-0002](../architecture/adr/0002-react-typescript-frontend.md)).
- **PostgreSQL** — транзакционные справочники и метаданные; отдельно от потоковой телеметрии ([ADR-0003](../architecture/adr/0003-use-postgres.md)).
- **ClickHouse** — большие объёмы временных рядов, аналитика и поиск событий (NFR-02); запись из SavingService, чтение из Web API ([ADR-0004](../architecture/adr/0004-clickhouse-telemetry.md)).
- **Redis** — кэш последних показаний и сессии без перегрузки СУБД; HA — реплика и **Redis Sentinel** ([ADR-0008](../architecture/adr/0008-redis-sentinel-cache-sessions.md)).
- **RabbitMQ с MQTT-плагином** — MQTT с контроллеров и AMQP для сервисов в одном контуре ([ADR-0005](../architecture/adr/0005-rabbitmq-mqtt-broker.md)).
- **MinIO (S3 API)** — объектное хранилище для крупного архива видео; контракт S3 для go2rtc ([ADR-0006](../architecture/adr/0006-minio-s3-object-storage.md)).
- **HashiCorp Vault** — централизованные секреты приложений по Vault API ([ADR-0007](../architecture/adr/0007-hashicorp-vault-secrets.md)).
- **go2rtc, RTSP/WebRTC, reverse proxy** — заданы диаграммой и расчётом нагрузки; отдельные ADR не оформлены.

## Клиентские приложения

| Технология | Где используется | Почему выбрана |
| --- | --- | --- |
| React 18.2.0, TypeScript | `CNT_GM_Web`, `CNT_GM_Identity_Web` | Единый типизированный SPA-стек; согласование с GraphQL/WebSocket и OIDC; версия React из метаданных Web. [ADR-0002](../architecture/adr/0002-react-typescript-frontend.md) |

## Серверные приложения и API

| Технология | Где используется | Почему выбрана |
| --- | --- | --- |
| ASP.NET Core, .NET 10 | `CNT_GM_WebAPI`, `CNT_GM_SavingService`, `CNT_GM_Identity_WebAPI` | Один стек для stateless-сервисов за балансировщиком, общие практики деплоя и безопасности. [ADR-0001](../architecture/adr/0001-dotnet-aspnet-core-backend.md) |
| OpenIddict | `CNT_GM_Identity_WebAPI` (поверх ASP.NET Core) | Стандартный OIDC/OAuth2 на той же платформе, что и основной API. [ADR-0001](../architecture/adr/0001-dotnet-aspnet-core-backend.md) |

## Протоколы и API между компонентами

| Технология | Назначение | Почему выбрана |
| --- | --- | --- |
| HTTPS, WebSocket, GraphQL | Клиент ↔ Web API; Bearer access token | Единая точка API для UI; идентификаторы сущностей в REST — **UUID** ([gm_openapi.yaml](../architecture/openapi/components/gm_openapi.yaml)); GraphQL для запросов к телеметрии; WebSocket для живых данных — по моделям связей `CNT_GM_Web` ↔ `CNT_GM_WebAPI`. |
| OpenID Connect | Аутентификация Web-приложения с Identity | Стандарт для браузерных SPA и выдачи access token; согласован с OpenIddict на сервере. [ADR-0001](../architecture/adr/0001-dotnet-aspnet-core-backend.md), [ADR-0002](../architecture/adr/0002-react-typescript-frontend.md) |
| HTTPS, OIDC metadata, JWKS | Web API ↔ Identity Web API | Проверка access token без общей сессии; stateless API. [ADR-0001](../architecture/adr/0001-dotnet-aspnet-core-backend.md) |
| TLS/MQTT | Контроллеры теплицы → брокер | Протокол IoT по требованиям и [calc_architecture.md](../architecture/calc_architecture.md); вход в единый брокер. [ADR-0005](../architecture/adr/0005-rabbitmq-mqtt-broker.md) |
| AMQP (5672/TCP via TLS) | `CNT_GM_SavingService` → брокер | Внутренние потребители на AMQP без второго брокера рядом с MQTT. [ADR-0005](../architecture/adr/0005-rabbitmq-mqtt-broker.md) |
| HTTPS, Vault API | Сервисы → секреты | Централизованная выдача секретов и аудит вместо разрозненных конфигов. [ADR-0007](../architecture/adr/0007-hashicorp-vault-secrets.md) |
| 6379/TCP/RESP via TLS | `CNT_GM_WebAPI`, `CNT_GM_SavingService` ↔ `CNT_GM_Redis_DB` | Кэш онлайн-показаний и сессии stateless API; отказоустойчивость — primary/replica и Sentinel ([ADR-0008](../architecture/adr/0008-redis-sentinel-cache-sessions.md)). |

## Обмен сообщениями

| Технология | Назначение | Почему выбрана |
| --- | --- | --- |
| RabbitMQ с MQTT-плагином | Внешний MQTT для устройств; внутренние подписчики по AMQP | Один продукт на MQTT-периметре и AMQP для .NET-сервисов; оценка ~1000 MQTT-сессий и HA из [calc_architecture.md](../architecture/calc_architecture.md). [ADR-0005](../architecture/adr/0005-rabbitmq-mqtt-broker.md) |

## Данные и кэш

| Технология | Назначение | Почему выбрана |
| --- | --- | --- |
| PostgreSQL | Метаданные и справочники (`CNT_GM_DB`); Identity (`CNT_GM_Identity_DB`) | ACID, связи, долговременные справочники; отдельно от телеметрии; одна семья СУБД с Identity. [ADR-0003](../architecture/adr/0003-use-postgres.md) |
| ClickHouse | Телеметрия и аналитика датчиков (`CNT_GM_Timeseries_DB`); порты: 8123 (запись), 8443 (API из Web API по модели) | Колоночное хранение временных рядов и запросы для поиска событий (NFR-02); объёмы в [calc_architecture.md](../architecture/calc_architecture.md). [ADR-0004](../architecture/adr/0004-clickhouse-telemetry.md) |
| Redis + Redis Sentinel | Кэш последних показаний, сессии (`CNT_GM_Redis_DB`) | Низкая задержка для онлайн-показаний и сессий stateless Web API; реплика и Sentinel для failover ([ADR-0008](../architecture/adr/0008-redis-sentinel-cache-sessions.md)). |

## Видео

| Технология | Назначение | Почему выбрана |
| --- | --- | --- |
| go2rtc | Живое и архивное видео (`CNT_GM_WebRTCServer_Only`, `CNT_GM_WebRTCServer_History`) | Готовый слой RTSP → WebRTC/MSE для браузера; разделение live и history в C4 (отдельный ADR не оформлен). |
| RTSP (554/TCP), UDP RTC (5004, 5005) | Камеры → go2rtc | Типичный транспорт IP-камер; соответствует модели связей с `CNT_Video_Camera`. |
| WebSocket, MSE | Браузер ↔ go2rtc | Доставка видео в SPA по модели `CNT_GM_Web`. [ADR-0002](../architecture/adr/0002-react-typescript-frontend.md) (клиент) |
| MinIO (S3 API) | Архив сегментов видео (`CNT_GM_S3`) | Большие объёмы архива; S3 API для записи/чтения сегментов go2rtc; масштаб и lifecycle в [calc_architecture.md](../architecture/calc_architecture.md). [ADR-0006](../architecture/adr/0006-minio-s3-object-storage.md) |

## Инфраструктура и секреты

| Технология | Назначение | Почему выбрана |
| --- | --- | --- |
| HashiCorp Vault | Хранение секретов (`CNT_GM_Secrets`) | Единая точка Vault API для Web API и медиа-сервисов; политики и аудит. [ADR-0007](../architecture/adr/0007-hashicorp-vault-secrets.md) |
| Reverse proxy / L7 load balancer | TLS на периметре, балансировка Web и API | TLS termination и несколько инстансов stateless API (NFR-03); описано в моделях Web и Web API (отдельный ADR не оформлен). |

## Периметр теплицы (устройства)

| Технология | Назначение | Почему выбрана |
| --- | --- | --- |
| MQTT (TLS) | Публикация телеметрии с цифрового контроллера в брокер | Стандарт для IoT и расчёта постоянных соединений; согласовано с RabbitMQ MQTT. [ADR-0005](../architecture/adr/0005-rabbitmq-mqtt-broker.md) |
