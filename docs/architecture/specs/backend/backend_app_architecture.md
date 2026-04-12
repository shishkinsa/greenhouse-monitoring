# Требования к архитектуре приложения backend

## Требование к структуре приложения

Приложение backend должно быть построено по чистой архитектуре и иметь следующие папки и сборки.

### Требования к структуре папок

В папе приложения создаются следующие папки

```text
- 0 Utils
  - [GM].[Название сервиса].[Utils]
- 1 Entities
  - [GM].[Название сервиса].[Entities]
- 2 Infrastructure.Interfaces
  - [GM].[Название сервиса].[DataAcces].[Interfaces]
  - [GM].[Название сервиса].[RabbitMQ].[Interfaces]
- 3 UseCases
  - [GM].[Название сервиса].[UseCases]
- 4 Controllers
  - [GM].[Название сервиса].[Controllers]
- 5 Infrastructure.Implementation
  - [GM].[Название сервиса].[DataAcces].[Postgres]
  - [GM].[Название сервиса].[RabbitMQ].[Implementation]
- 6 WebApp
  - [GM].[Название сервиса].[WebApp]
- 7 Tests
  - [GM].[Название сервиса].[Tests]
```

**0 Utils**
Папка содержи сборку с общими методами, расширениями для приложения.

**1 Entities**
``[GM].[Название сервиса].[Entities]`` - Не содержит интерфейсы репозитория. Содержит только Entities. Должна быть Rich модель

**2 Infrastructure.Interfaces**
Содержит ссылку на Microsoft.EntityFrameworkCore.
Как пример должна содержать:

- ORM для доступа к данным.
- Сервис получения текущего пользователя.
- Доступ к данным.

**3 UseCases**
``[GM].[Название сервиса].[UseCases]`` - содержит пользовательские сценарии. Подход CQRS. Используем MediatoR

**4 Controllers**
Данный слой используется если необходимо выделить контроллеры, например, для мобильного приложения.

**5 Infrastructure.Implementation**
``[GM].[Название сервиса].[DataAcces].[Postgres]`` - содержит ссылку на конкретный Framework. Для нашего проекта Npgsql.EntityFrameworkCore.PosgreSQL
``[GM].[Название сервиса].[RabbitMQ].[Implementation]`` - содержит реализацию доступа к RabbitMQ

**6 WebApp**
``[GM].[Название сервиса].[WebApp]`` - используется только слой UseCases. Используется Requestum как альтернатива коммерческого продукта MediatR.

**7 Tests**
Размещение Unit Tests для приложения.
