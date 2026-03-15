# Соглашения по именованию React

Правила задаются данным документом. Базовые соглашения TypeScript — [ts-naming-conventions.md](ts-naming-conventions.md). Общие стандарты проекта — [src/.editorconfig](src/.editorconfig).

| Сущность | Стиль | Пример |
|----------|--------|--------|
| Компоненты | PascalCase, существительное | UserProfile, PaymentForm |
| Файлы компонентов | PascalCase.tsx | UserProfile.tsx |
| Props-интерфейсы | ComponentName + Props | UserProfileProps |
| Хуки | use + camelCase | useAuth, useDebounce |
| Файлы хуков | use-название.ts (kebab-case) | use-auth.ts |
| Обработчики | handle + Элемент + Событие | handleSubmit, handleClick |
| Пропсы-колбэки | on + Элемент + Событие | onSubmit, onUserSelect |
| useState | [сущность, setСущность] | [user, setUser] |

---

## Компоненты

```typescript
// ✅ ПРАВИЛЬНО - PascalCase, существительное
const UserProfile: React.FC = () => {
    return <div>Profile</div>;
};

const PaymentForm: React.FC<PaymentFormProps> = ({ onSubmit }) => {
    return <form onSubmit={onSubmit}>...</form>;
};

const OrderList: React.FC = () => {
    return <div>Orders</div>;
};

// ❌ НЕПРАВИЛЬНО
const userProfile: React.FC = () => {        // Не с маленькой
    return <div>Profile</div>;
};

const getPaymentForm: React.FC = () => {      // Глагол, а должен быть компонент
    return <form>...</form>;
};
```

## Файлы компонентов

```typescript
// ✅ ПРАВИЛЬНО - PascalCase.tsx
UserProfile.tsx
PaymentForm.tsx
OrderList.tsx
AuthGuard.tsx

// ❌ НЕПРАВИЛЬНО
userProfile.tsx            // Не с маленькой
payment-form.tsx           // kebab-case не для компонентов
index.tsx                  // Только для реэкспорта
```

## Props интерфейсы

```typescript
// ✅ ПРАВИЛЬНО - ComponentName + Props
interface UserProfileProps {
    userId: string;
    showDetails?: boolean;
    onUpdate?: (user: User) => void;
}

interface PaymentFormProps {
    amount: number;
    currency: string;
    onSubmit: (data: PaymentData) => void;
}

const UserProfile: React.FC<UserProfileProps> = ({ userId, showDetails }) => {
    return <div>...</div>;
};

// ❌ НЕПРАВИЛЬНО
interface Props {                             // Слишком обще
    userId: string;
}

interface userProfileProps {                   // Не с большой
    userId: string;
}
```

## Хуки

```typescript
// ✅ ПРАВИЛЬНО - use + camelCase
function useAuth() {
    const [user, setUser] = useState<User | null>(null);
    return { user, login, logout };
}

function useLocalStorage<T>(key: string, initialValue: T) {
    // ...
}

function useDebounce<T>(value: T, delay: number): T {
    // ...
}

// ❌ НЕПРАВИЛЬНО
function auth() {                              // Без префикса use
    const [user, setUser] = useState(null);
}

function useLocalStorageValue() {               // Слишком длинно
    // ...
}
```

## Файлы хуков

```typescript
// ✅ ПРАВИЛЬНО - use-название.ts (kebab-case)
use-auth.ts
use-local-storage.ts
use-debounce.ts
use-click-outside.ts

// ❌ НЕПРАВИЛЬНО
useAuth.ts                    // CamelCase в именах файлов не принят
auth-hook.ts                  // use- префикс должен быть
```

## Обработчики событий

```typescript
// ✅ ПРАВИЛЬНО - handle + ИмяЭлемента + Событие
const handleSubmit = (e: FormEvent) => {};
const handleClick = (e: MouseEvent) => {};
const handleInputChange = (e: ChangeEvent<HTMLInputElement>) => {};
const handleUserSelect = (user: User) => {};

// В пропсах - on + ИмяЭлемента + Событие
interface Props {
    onSubmit: (data: FormData) => void;
    onClick: () => void;
    onUserSelect: (user: User) => void;
}

// ❌ НЕПРАВИЛЬНО
const submitHandler = (e: FormEvent) => {};      // Нестандартно
const onChange = (e: ChangeEvent) => {};          // Слишком обще
```

## Состояние (useState)

```typescript
// ✅ ПРАВИЛЬНО - [сущность, setСущность]
const [user, setUser] = useState<User | null>(null);
const [isLoading, setIsLoading] = useState(false);
const [formData, setFormData] = useState<FormData>(initialData);
const [errors, setErrors] = useState<ValidationErrors>({});

// ❌ НЕПРАВИЛЬНО
const [user, updateUser] = useState(null);        // Не setUser
const [loading, set_loading] = useState(false);    // snake_case
const [data, setData] = useState([]);              // Слишком обще
```

## useEffect зависимости

```typescript
// ✅ ПРАВИЛЬНО - явно указывать зависимости
useEffect(() => {
    fetchUser(userId);
}, [userId]);  // Только нужные зависимости

useEffect(() => {
    const subscription = api.subscribe(userId, handleUpdate);
    return () => subscription.unsubscribe();
}, [userId, handleUpdate]);

// ❌ НЕПРАВИЛЬНО
useEffect(() => {
    fetchUser(userId);
}, []);  // Missing dependency

useEffect(() => {
    // Эффект
}, [props]);  // Слишком обще
```

## Условный рендеринг

```typescript
// ✅ ПРАВИЛЬНО - явные булевы переменные
const UserDashboard: React.FC = () => {
    const isLoading = !user && !error;
    const hasError = !!error;
    const hasUser = !!user;

    if (isLoading) return <Spinner />;
    if (hasError) return <ErrorMessage error={error} />;
    if (!hasUser) return <Navigate to="/login" />;

    return <UserProfile user={user} />;
};

// ❌ НЕПРАВИЛЬНО
return (
    <div>
        {!user && !error ? <Spinner /> :
         error ? <ErrorMessage /> :
         user ? <UserProfile /> : null}
    </div>
);
```
