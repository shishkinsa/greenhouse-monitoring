# Исходный код `cnt_gm_web`

Структура слоёв соответствует целевой модели **Feature-Sliced Design**, зафиксированной в [12-frontend-app-architecture.md](../../../../docs/architecture/specs/frontend/12-frontend-app-architecture.md).

| Каталог | Слой FSD |
|---------|-----------|
| `app/` | Инициализация: провайдеры, роутинг, глобальные стили |
| `pages/` | Страницы по маршрутам |
| `widgets/` | Крупные составные блоки (оболочка приложения и т.п.) |
| `features/` | Сценарии пользователя (загрузка данных и т.д.) |
| `entities/` | Домен в UI (таблица прогноза, типы, API сущности) |
| `shared/` | Утилиты без домена (`api/http`, тема Ant) |

Импорты c префиксом `@/` настроены на каталог `src/`. Верхние слои не должны импортировать код «вверх» по иерархии (например, `shared` не тянет `entities`).

Документирование публичного API (TSDoc): см. подраздел **«Документирование frontend (React, FSD)»** в [coding-standards.md](../../../../docs/standards/coding-standards.md).

---

## Как устроена навигация

1. **`app/main.tsx`** — монтирование React, глобальные стили, провайдеры.
2. **`app/router.tsx`** — соответствие пути URL и компонента страницы.
3. **`widgets/app-layout`** — рамка (меню, шапка, подвал); в середине **`Outlet`** подставляется страница текущего маршрута.
4. **`pages/...`** — экран: собирает UI, подключает хуки из **`features`**, данные из **`entities`**.

Страница по возможности **тонкая**: сценарии («нажал — загрузил») — в **features**, типы и запросы к API сущности — в **entities**, общий HTTP — в **shared**.

---

## Слои: куда что класть

| Слой | Назначение | Примеры в проекте |
|------|------------|-------------------|
| **`app`** | Вход, дерево маршрутов, стили приложения | `main.tsx`, `router.tsx`, `providers.tsx` |
| **`pages`** | Один URL — одна папка со страницей | `pages/home`, `pages/weather` |
| **`widgets`** | Крупные блоки, переиспользуемые между страницами | `widgets/app-layout` |
| **`features`** | Сценарий: состояние, вызов API, уведомления | `features/weather/load-forecast` |
| **`entities`** | Сущность: типы, `fetch`, таблица/карточка | `entities/weather` |
| **`shared`** | Без домена | `shared/api/http`, `shared/config/theme` |

**Импорт только «вниз» по слоям.** `shared` не импортирует `entities` или `pages`. Страница может импортировать `features`, `entities`, `shared`, `widgets`.

---

## Как добавить новую страницу (чеклист)

Пример: страница **`/greenhouses`** («Теплицы»). Имена замените на свои.

### 1. Маршрут

Файл **`app/router.tsx`**:

- Импортируйте страницу, например `import { GreenhousesPage } from '@/pages/greenhouses';`
- Внутри `Route` с `element={<AppLayout />}` добавьте:

```tsx
<Route path="greenhouses" element={<GreenhousesPage />} />
```

Вложенные маршруты без ведущего `/` в `path` дают путь от корня родителя: при родителе `path="/"` итоговый URL будет **`/greenhouses`**. Если используете абсолютный путь в дочернем `Route`, согласуйте его с [документацией React Router](https://reactrouter.com/).

### 2. Папка страницы (`pages`)

- Создайте, например: `pages/greenhouses/ui/GreenhousesPage.tsx` — компонент страницы (как `HomePage`, `WeatherPage`).
- Добавьте **`pages/greenhouses/index.ts`**:

```ts
export { GreenhousesPage } from './ui/GreenhousesPage';
```

### 3. Пункт бокового меню

Файл **`widgets/app-layout/AppLayout.tsx`**, массив **`items`**: добавьте элемент с **`Link`** на тот же путь, что и в роуте (`to="/greenhouses"`), и **`key`**, совпадающий с этим путём (как у `/home` и `/weather`).

### 4. Данные с API (если нужны)

1. **`entities/<сущность>/model/types.ts`** — типы ответа backend.
2. **`entities/<сущность>/api/*.ts`** — функции запросов через **`httpJson`** из `@/shared/api/http`, URL вида **`/api/...`** (в dev прокси в `vite.config.ts`).
3. **`entities/<сущность>/index.ts`** — реэкспорт публичного API слайса.
4. **`features/<область>/<действие>/model/use*.ts`** — хук: состояние, вызов `fetch`, при ошибках — **`App.useApp().message`** (нужна обёртка из **`AppProviders`**).
5. Страница в **`pages/...`** только вызывает хук и рендерит Ant-компоненты.

Статическая страница без API — шаг 4 можно пропустить.

### 5. Соглашения

- TSDoc для публичных экспортов — [coding-standards.md](../../../../docs/standards/coding-standards.md).
- Имена React — [react-naming-conventions.md](../../../../docs/standards/react-naming-conventions.md).

### 6. Проверка

- `npm run dev` — переход по новому URL.
- `npm run build` — без ошибок TypeScript.

---

## Поток данных (пример «Погода»)

```
shared/api/http.ts          ← общий fetch + JSON
        ↑
entities/weather/api/       ← fetchWeatherForecast()
        ↑
features/weather/load-forecast   ← хук useLoadWeatherForecast
        ↑
pages/weather/              ← WeatherPage: кнопка + таблица
```

---

## Импорты

Используйте алиас **`@/`** (каталог `src/`):

```ts
import { X } from '@/entities/weather';
import { useLoadWeatherForecast } from '@/features/weather/load-forecast';
```

---

## Типичные ошибки

- Есть маршрут, нет пункта в меню — страница доступна только по прямому URL.
- Несовпадение **`path` в `router`** и **`to` / `key` в меню** — пункт не подсвечивается или ведёт не туда.
- Большой **`fetch`** только в странице — лучше вынести в **`entities`** и **`features`**.
- Хук с **`App.useApp()`** без дерева **`AppProviders`** — `message`/`notification` не работают.
- Импорт «снизу вверх» (например, из `entities` в `pages` запрещён наоборот: `entities` не должен тянуть `pages`).

---

## Ориентиры в репозитории

- Разметка маршрутов: **`app/router.tsx`**
- Меню: **`widgets/app-layout/AppLayout.tsx`**
- Примеры страниц: **`pages/home`**, **`pages/weather`**
- Пример с API: **`entities/weather`**, **`features/weather/load-forecast`**
