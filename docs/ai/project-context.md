# Контекст проекта: узлы диаграммы контейнеров

**Greenhouse Monitoring** — система, которая получает данные по датчикам и видеопотоку с теплицы и предоставляет к ним доступ пользователям. Ниже сводка узлов уровня контейнеров (C4): приложения, сервисы, брокер, хранилища и периметр теплицы. Назначения согласованы с моделями LikeC4 в `docs/architecture/diagram/containers/`; при изменении архитектуры обновляйте сначала эти модели, затем при необходимости этот документ.

**Идентификаторы домена:** организации, теплицы, датчики, камеры, события аналитики и ссылки в ClickHouse — **`uuid`** в PostgreSQL/HTTP API (см. `docs/architecture/openapi/components/gm_openapi.yaml`, `docs/architecture/diagram/data/cnt_gm_db/metadata_database_structure.md`). На объекте теплицы по-прежнему используется стабильный **`greenhouse_code`** для MQTT и онбординга.

| Идентификатор | Тип | Технология | Назначение |
| --- | --- | --- | --- |
| Теплица | Периметр теплицы | — | Площадка выращивания растений; внешний контекст для устройств и обмена данными с центральной системой. |
| CNT_Digital_Controller | Устройство (контроллер) | — | Сбор данных с датчиков температуры, влажности и кислотности почвы; публикация телеметрии в брокер (MQTT). |
| CNT_Video_Camera | Устройство (камера) | — | Сбор видеоданных; источник RTSP/WebRTC для серверов go2rtc. |
| CNT_GM_Web | Клиентское приложение | React, TypeScript | Web-приложение ИС: статика и SPA за reverse proxy/LB (TLS на периметре); оперативный и исторический просмотр данных, вызовы API и видеопотоков. |
| CNT_GM_Identity_Web | Клиентское приложение | React, TypeScript | Web-приложение подсистемы Identity. |
| CNT_GM_WebAPI | Сервис API | ASP.NET Core, .NET 10 | Web API: stateless-инстансы за балансировщиком; GraphQL/HTTPS/WebSocket к клиенту; метаданные и справочники в PostgreSQL, поиск телеметрии в ClickHouse, кэш и сессии в Redis, проверка токенов у Identity, секреты из Vault. |
| CNT_GM_Identity_WebAPI | Сервис API | OpenIddict, ASP.NET Core, .NET 10 | Сервис Identity Web API: выдача и проверка учётных данных, работа с БД Identity. |
| CNT_GM_Broker | Брокер сообщений | RabbitMQ (MQTT plugin) | Приём телеметрии от контроллеров по MQTT; внутренним сервисам — подписка по AMQP. |
| CNT_GM_SavingService | Фоновый сервис | ASP.NET Core, .NET 10 | Чтение потока датчиков из брокера, запись телеметрии в ClickHouse и обновление кэша последних показаний в Redis. |
| CNT_GM_WebRTCServer_Only | Сервис видео | go2rtc | WebRTC для живого видео с камер; получение секретов из Vault. |
| CNT_GM_WebRTCServer_History | Сервис видео | go2rtc | WebRTC для архивного видео: запись/чтение сегментов в S3-совместимом хранилище, доступ к камерам; секреты из Vault. |
| CNT_GM_DB | База данных | PostgreSQL | Справочники и метаданные приложения (CRUD через Web API). |
| CNT_GM_Identity_DB | База данных | PostgreSQL | Данные аутентификации и авторизации Identity. |
| CNT_GM_Timeseries_DB | База данных | ClickHouse | Хранение и анализ телеметрии с датчиков; запись SavingService, чтение/поиск через Web API. |
| CNT_GM_Redis_DB | Кэш | Redis | Кэш последних показаний датчиков (пишет SavingService, читает Web API), сессии. |
| CNT_GM_S3 | Хранилище объектов | MinIO | Архив видео с камер (S3 API). |
| CNT_GM_Secrets | Хранилище секретов | HashiCorp Vault | Централизованное хранение секретов для сервисов. |
