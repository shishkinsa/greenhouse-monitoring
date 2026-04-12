# Соглашения по именованию PostgreSQL

Правила именования для PostgreSQL задаются данным документом. Общие стандарты проекта (C#) — [src/.editorconfig](src/.editorconfig). В .editorconfig нет секции для SQL; проверка соответствия — по ревью и при необходимости линтерами/скриптами миграций.

**Проект greenhouse-monitoring:** в `CNT_GM_DB` и в публичном API ([gm_openapi.yaml](../architecture/openapi/components/gm_openapi.yaml)) первичные и внешние ключи доменных сущностей — тип **`uuid`**. Примеры ниже с `BIGSERIAL`/`SERIAL` сохранены как **универсальные** шаблоны SQL; для таблиц этого продукта применяйте `uuid PRIMARY KEY DEFAULT gen_random_uuid()` (или эквивалент), если иное не зафиксировано в [20-metadata-database-structure.md](../architecture/diagram/data/cnt_gm_db/20-metadata-database-structure.md).

| Сущность | Стиль | Пример |
|----------|--------|--------|
| База данных | snake_case | company_production |
| Схема | lowercase | app, audit, billing |
| Таблица | множественное число, snake_case | users, order_items |
| Колонка | snake_case | id, created_at, is_active |
| Индекс | idx_таблица_колонки | idx_users_email |
| Внешний ключ (constraint) | fk_таблица_родитель | fk_orders_users |
| Последовательность | таблица_колонка_seq | users_id_seq |
| Представление | v_имя | v_active_users |
| Функция / процедура | глагол_существительное, snake_case | get_user_by_email |
| Триггер | tr_таблица_событие | tr_orders_before_insert |

---

## База данных

```sql
-- ✅ ПРАВИЛЬНО - snake_case, описательно
CREATE DATABASE company_production;
CREATE DATABASE company_staging;
CREATE DATABASE company_testing;

-- ❌ НЕПРАВИЛЬНО
CREATE DATABASE CompanyProd;      -- CamelCase в SQL не принят
CREATE DATABASE myapp;            -- Слишком обще
CREATE DATABASE db1;               -- Бессмысленно
```

## Схемы

```sql
-- ✅ ПРАВИЛЬНО - группировка по доменам
CREATE SCHEMA app;          -- Основные таблицы приложения
CREATE SCHEMA audit;        -- Аудит и логи
CREATE SCHEMA billing;       -- Платежный домен
CREATE SCHEMA reference;     -- Справочники

-- ❌ НЕПРАВИЛЬНО
CREATE SCHEMA schema1;       -- Неинформативно
CREATE SCHEMA new_schema;    -- Временное имя
```

## Таблицы

```sql
-- ✅ ПРАВИЛЬНО - множественное число, snake_case
CREATE TABLE users (
    id BIGSERIAL PRIMARY KEY,
    email VARCHAR(255) UNIQUE NOT NULL
);

CREATE TABLE user_roles (                    -- Связующая таблица
    user_id BIGINT REFERENCES users(id),
    role_id BIGINT REFERENCES roles(id)
);

CREATE TABLE order_items (                    -- Детали заказа
    order_id BIGINT REFERENCES orders(id),
    product_id BIGINT REFERENCES products(id),
    quantity INT NOT NULL
);

-- ❌ НЕПРАВИЛЬНО
CREATE TABLE user (                           -- Единственное число
CREATE TABLE UsersTable                        -- С венгерской нотацией
CREATE TABLE order_item                         -- Единственное число
```

## Колонки

```sql
-- ✅ ПРАВИЛЬНО - snake_case, без префиксов таблицы
CREATE TABLE users (
    id BIGSERIAL PRIMARY KEY,                   -- Просто id, не user_id
    email VARCHAR(255) UNIQUE NOT NULL,
    full_name VARCHAR(100),                      -- Полное имя
    created_at TIMESTAMP DEFAULT NOW(),
    updated_at TIMESTAMP,
    is_active BOOLEAN DEFAULT true
);

-- Внешние ключи - имя таблицы в единственном числе + _id
ALTER TABLE orders ADD COLUMN user_id BIGINT REFERENCES users(id);
ALTER TABLE order_items ADD COLUMN order_id BIGINT REFERENCES orders(id);

-- ❌ НЕПРАВИЛЬНО
CREATE TABLE users (
    UserID SERIAL PRIMARY KEY,                   -- CamelCase
    user_email VARCHAR(255),                      -- Избыточно (и так в users)
    fld_created TIMESTAMP,                         -- Венгерская нотация
    flag BOOLEAN                                    -- Непонятно что
);
```

## Индексы

```sql
-- ✅ ПРАВИЛЬНО - idx_таблица_колонка
CREATE INDEX idx_users_email ON users(email);
CREATE INDEX idx_orders_user_id ON orders(user_id);
CREATE INDEX idx_orders_created_at ON orders(created_at);

-- Составной индекс
CREATE INDEX idx_orders_user_created ON orders(user_id, created_at);

-- Уникальный индекс
CREATE UNIQUE INDEX idx_users_email_unique ON users(email);

-- ❌ НЕПРАВИЛЬНО
CREATE INDEX email_idx ON users(email);           -- Непонятно таблица
CREATE INDEX index_on_users_email ON users(email); -- Слишком длинно
CREATE INDEX idx1 ON orders(user_id);              -- Неинформативно
```

## Внешние ключи

```sql
-- ✅ ПРАВИЛЬНО - fk_таблица_родитель
ALTER TABLE orders 
ADD CONSTRAINT fk_orders_users 
FOREIGN KEY (user_id) REFERENCES users(id);

ALTER TABLE order_items 
ADD CONSTRAINT fk_order_items_orders 
FOREIGN KEY (order_id) REFERENCES orders(id);

-- ❌ НЕПРАВИЛЬНО
ALTER TABLE orders 
ADD CONSTRAINT user_fk                                 -- Непонятно
FOREIGN KEY (user_id) REFERENCES users(id);
```

## Последовательности (Sequences)

```sql
-- ✅ ПРАВИЛЬНО - таблица_колонка_seq
CREATE SEQUENCE users_id_seq;
CREATE SEQUENCE orders_id_seq;

-- ❌ НЕПРАВИЛЬНО
CREATE SEQUENCE seq1;
CREATE SEQUENCE users_sequence;
```

## Представления (Views)

```sql
-- ✅ ПРАВИЛЬНО - v_имя (описательно)
CREATE VIEW v_active_users AS
SELECT * FROM users WHERE is_active = true;

CREATE VIEW v_order_summary AS
SELECT o.id, u.email, o.total, o.created_at
FROM orders o
JOIN users u ON u.id = o.user_id;

-- ❌ НЕПРАВИЛЬНО
CREATE VIEW active_users_view;      -- Избыточно view
CREATE VIEW v1;                     -- Непонятно
```

## Функции и процедуры

```sql
-- ✅ ПРАВИЛЬНО - глагол_существительное
CREATE FUNCTION get_user_by_email(email TEXT)
CREATE FUNCTION calculate_order_total(order_id BIGINT)
CREATE PROCEDURE update_user_status(user_id BIGINT, is_active BOOLEAN)

-- ❌ НЕПРАВИЛЬНО
CREATE FUNCTION func1()
CREATE PROCEDURE sp_update_user()   -- Венгерская нотация
```

## Триггеры

```sql
-- ✅ ПРАВИЛЬНО - tr_таблица_событие (before/after + insert/update/delete)
CREATE TRIGGER tr_orders_before_insert
    BEFORE INSERT ON orders
    FOR EACH ROW EXECUTE FUNCTION set_created_at();

CREATE TRIGGER tr_users_after_update
    AFTER UPDATE ON users
    FOR EACH ROW EXECUTE FUNCTION audit_user_changes();

-- ❌ НЕПРАВИЛЬНО
CREATE TRIGGER trigger1                    -- Неинформативно
CREATE TRIGGER orders_insert_trigger       -- Лучше tr_orders_before_insert
```
