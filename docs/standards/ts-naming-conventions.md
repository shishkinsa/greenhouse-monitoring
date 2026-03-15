# Соглашения по именованию TypeScript

Правила именования задаются данным документом. Общие стандарты проекта (C#) — [src/.editorconfig](src/.editorconfig).

| Сущность | Стиль | Пример |
|----------|--------|--------|
| Файлы | kebab-case | user-service.ts, use-auth.ts |
| Папки | kebab-case | features/, auth/, shared/ |
| Интерфейсы | PascalCase, без префикса I | User, PaymentRequest |
| Типы (type) | PascalCase | UserId, OrderStatus |
| Классы | PascalCase | UserService |
| Переменные | camelCase | userName, isActive |
| Константы | UPPER_SNAKE_CASE | MAX_RETRY_COUNT, API_CONFIG |
| Функции | camelCase | formatDate, handleSubmit |
| Параметры | camelCase | userId, profile |
| Enum | PascalCase (тип и значения) | OrderStatus.Pending |
| Generics | T, K, V или описательно | T, KeyValuePair |

---

## Файлы

```typescript
// ✅ ПРАВИЛЬНО - kebab-case, описательно
user-service.ts           // Сервис
payment.types.ts          // Типы
api-client.ts             // Клиент API
use-auth.ts               // Хук
format-date.util.ts       // Утилита

// ❌ НЕПРАВИЛЬНО
userService.ts            // CamelCase в именах файлов не принят
paymentTypes.ts           // Должен быть kebab-case
utils.ts                  // Слишком обще
index.ts                  // Только для точек входа
```

## Папки

```typescript
// ✅ ПРАВИЛЬНО - kebab-case
src/
├── features/
│   ├── auth/
│   ├── payments/
│   └── users/
├── shared/
│   ├── components/
│   ├── hooks/
│   └── utils/
└── types/

// ❌ НЕПРАВИЛЬНО
src/
├── feature/              // Единственное число
├── Auth/                 // С большой буквы
└── __tests__/            // Спецсимволы
```

## Интерфейсы

```typescript
// ✅ ПРАВИЛЬНО - PascalCase, без префикса I (в отличие от C#)
interface User {
    id: string;
    email: string;
    name: string;
}

interface PaymentRequest {
    amount: number;
    currency: string;
    userId: string;
}

interface ApiResponse<T> {
    data: T;
    status: number;
    message?: string;
}

// ❌ НЕПРАВИЛЬНО
interface IUser {                 // I-префикс из C# не нужен в TS
    id: string;
}

interface userInterface {          // Слишком длинно
    id: string;
}
```

## Типы (Type Aliases)

```typescript
// ✅ ПРАВИЛЬНО - PascalCase
type UserId = string;
type OrderStatus = 'pending' | 'paid' | 'shipped';
type Callback = (error: Error | null, result?: any) => void;
type DeepPartial<T> = { [P in keyof T]?: DeepPartial<T[P]> };

// ❌ НЕПРАВИЛЬНО
type userId = string;              // Не с большой
type status = 'pending' | 'paid';   // Слишком обще
```

## Классы

```typescript
// ✅ ПРАВИЛЬНО - PascalCase
class UserService {
    private readonly apiClient: ApiClient;

    constructor(apiClient: ApiClient) {
        this.apiClient = apiClient;
    }

    async getUserById(id: string): Promise<User> {
        return this.apiClient.get(`/users/${id}`);
    }
}

// ❌ НЕПРАВИЛЬНО
class userService {                 // Не с большой
    constructor(ApiClient) {        // Параметр с большой
    }
}
```

## Переменные и константы

```typescript
// ✅ ПРАВИЛЬНО - camelCase для переменных, UPPER_SNAKE_CASE для констант
const userName = 'John';                    // camelCase
let isActive = true;                        // camelCase
const MAX_RETRY_COUNT = 3;                  // UPPER_SNAKE_CASE для констант
const DEFAULT_PAGE_SIZE = 20;                // UPPER_SNAKE_CASE

// Объекты-конфиги тоже UPPER_SNAKE_CASE
const API_CONFIG = {
    baseUrl: 'https://api.example.com',
    timeout: 5000
} as const;

// ❌ НЕПРАВИЛЬНО
const UserName = 'John';                    // PascalCase не для переменных
let is_active = true;                       // snake_case в JS не принят
const maxRetryCount = 3;                     // Константы должны быть UPPER_SNAKE_CASE
```

## Функции

```typescript
// ✅ ПРАВИЛЬНО - camelCase, глагол + существительное
function formatDate(date: Date): string {
    return date.toISOString();
}

const calculateTotal = (items: CartItem[]): number => {
    return items.reduce((sum, item) => sum + item.price, 0);
};

// Обработчики событий - handle + что
const handleSubmit = (event: FormEvent) => {};
const handleClick = () => {};

// ❌ НЕПРАВИЛЬНО
function FormatDate(date: Date): string {     // С большой
    return date.toISOString();
}

const calc = (items: CartItem[]) => {          // Слишком коротко
    return items.reduce((sum, item) => sum + item.price, 0);
};
```

## Параметры функций

```typescript
// ✅ ПРАВИЛЬНО - camelCase, описательно
function getUserById(userId: string): Promise<User> {}
function updateUserProfile(userId: string, profile: UserProfile) {}
function sendEmail(to: string, subject: string, body: string) {}

// ❌ НЕПРАВИЛЬНО
function getUserById(id: string): Promise<User> {}           // Слишком общо
function updateUserProfile(uId: string, p: UserProfile) {}   // Сокращения
```

## Enum

```typescript
// ✅ ПРАВИЛЬНО - PascalCase для enum, PascalCase для значений
enum OrderStatus {
    Pending = 'pending',
    Paid = 'paid',
    Shipped = 'shipped',
    Delivered = 'delivered'
}

enum ErrorCode {
    NotFound = 404,
    BadRequest = 400,
    Unauthorized = 401
}

// ❌ НЕПРАВИЛЬНО
enum orderStatus {                          // Не с большой
    PENDING,                                 // Капс значения
    paid,                                    // Разнобой
    order_status_3                           // snake_case
}
```

## Generics

```typescript
// ✅ ПРАВИЛЬНО - одна заглавная буква или описательно
interface ApiResponse<T> {
    data: T;
    status: number;
}

function identity<T>(arg: T): T {
    return arg;
}

// Для сложных случаев - описательно
type KeyValuePair<K extends string, V> = {
    key: K;
    value: V;
};

// ❌ НЕПРАВИЛЬНО
interface ApiResponse<Type> {                // Полное слово избыточно
    data: Type;
}
```
