# 💻 Стандарты кодирования

## 📋 Содержание
- [Общие принципы](#общие-принципы)
- [C# (.NET)](#c-net)
- [TypeScript](#typescript)
- [React](#react)
- [SQL](#sql)
- [Тестирование](#тестирование)
- [Документирование](#документирование)
- [Безопасность](#безопасность)
- [Производительность](#производительность)
- [Инструменты](#инструменты)

---

## 🎯 Общие принципы

### KISS (Keep It Simple, Stupid)
```csharp
// ❌ Плохо - излишне сложно
public bool IsValidUser(User user) 
{
    return user != null && user.Age >= 18 && user.Age <= 100 && 
           !string.IsNullOrEmpty(user.Email) && user.Email.Contains("@") &&
           (user.Role == "Admin" || user.Role == "User");
}

// ✅ Хорошо - просто и понятно
public bool IsValidUser(User user)
{
    return user != null 
        && IsAdult(user.Age)
        && IsValidEmail(user.Email)
        && HasValidRole(user.Role);
}
```

### DRY (Don't Repeat Yourself)
```csharp
// ❌ Плохо - дублирование
public class OrderService
{
    public void ProcessOrder(Order order)
    {
        if (order.Total < 0)
            throw new Exception("Order total cannot be negative");
        if (order.Items == null || !order.Items.Any())
            throw new Exception("Order must have items");
        // ...
    }
    
    public void ValidateOrder(Order order)
    {
        if (order.Total < 0)
            throw new Exception("Order total cannot be negative");
        if (order.Items == null || !order.Items.Any())
            throw new Exception("Order must have items");
        // ...
    }
}

// ✅ Хорошо - единый метод валидации
public class OrderService
{
    private void ValidateOrder(Order order)
    {
        if (order.Total < 0)
            throw new Exception("Order total cannot be negative");
        if (order.Items == null || !order.Items.Any())
            throw new Exception("Order must have items");
    }
    
    public void ProcessOrder(Order order)
    {
        ValidateOrder(order);
        // ...
    }
}
```

### YAGNI (You Ain't Gonna Need It)
```csharp
// ❌ Плохо - задел на будущее
public class PaymentProcessor
{
    private readonly ILogger _logger;
    private readonly ICache _cache;
    private readonly IQueue _queue;
    private readonly IAnalytics _analytics;
    
    // Используется только _logger, остальное "на всякий случай"
}

// ✅ Хорошо - только то, что нужно сейчас
public class PaymentProcessor
{
    private readonly ILogger _logger;
    
    // Добавляем зависимости только когда они реально нужны
}
```
---

## 🔷 C# (.NET)

### Форматирование

```csharp
// ✅ Правильно - открывающая скобка на новой строке
public class UserService
{
    public async Task<User> GetUserByIdAsync(Guid id)
    {
        var user = await _dbContext.Users
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync();
            
        return user;
    }
}

// Отступы - 4 пробела
// Пустые строки между логическими блоками
// Одна пустая строка в конце файла
```

### Using statements
```csharp
// ✅ Правильно - внутри namespace
namespace Company.Project.Services
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    
    public class UserService
    {
        // ...
    }
}

// Сортировка:
// 1. System.*
// 2. Third party
// 3. Project namespaces
```

### Модификаторы доступа
```csharp
public class UserService
{
    // Порядок модификаторов:
    // public → internal → protected → private
    // static → readonly
    
    private readonly IUserRepository _userRepository;
    private const int MaxRetryCount = 3;
    protected ILogger Logger { get; set; }
    
    public async Task<User> GetUserAsync(Guid id)
    {
        // ...
    }
}
```

### Обработка ошибок
```csharp
// ✅ Правильно - специфичные исключения
public async Task<User> GetUserAsync(Guid id)
{
    if (id == Guid.Empty)
    {
        throw new ArgumentException("Id must be non-empty", nameof(id));
    }
    
    var user = await _userRepository.GetByIdAsync(id);
    
    if (user == null)
    {
        throw new NotFoundException($"User with id {id} not found");
    }
    
    return user;
}

// Асинхронные методы должны принимать CancellationToken
public async Task<User> GetUserAsync(Guid id, CancellationToken cancellationToken = default)
{
    cancellationToken.ThrowIfCancellationRequested();
    // ...
}
```

### LINQ
```csharp
// ✅ Правильно - читаемые запросы
var activeUsers = users
    .Where(u => u.IsActive)
    .OrderBy(u => u.LastName)
    .ThenBy(u => u.FirstName)
    .Select(u => new UserDto
    {
        Id = u.Id,
        FullName = $"{u.FirstName} {u.LastName}"
    })
    .ToList();

// Форматирование - каждый оператор с новой строки
```

---

## 🔷 TypeScript

### Форматирование

```typescript
// ✅ Правильно - отступы 2 пробела
interface User {
  id: string;
  email: string;
  name: string;
}

function formatUserName(user: User): string {
  return `${user.name} (${user.email})`;
}

// Точки с запятой обязательны
// Одинарные кавычки
// Запятая в конце объектов
```

### Типизация
```typescript
// ✅ Правильно - явные типы
interface PaymentRequest {
  amount: number;
  currency: string;
  userId: string;
  metadata?: Record<string, unknown>;
}

type OrderStatus = 'pending' | 'paid' | 'shipped' | 'delivered';

// ✅ Использование utility types
type PartialUser = Partial<User>;
type ReadonlyUser = Readonly<User>;
type UserWithoutPassword = Omit<User, 'password'>;

// ❌ Плохо - any
function processData(data: any): any {
  return data.result;
}

// ✅ Хорошо - generics
function processData<T>(data: T): T {
  return data;
}
```

### Асинхронность
```typescript
// ✅ Правильно - async/await
async function fetchUser(id: string): Promise<User> {
  try {
    const response = await api.get(`/users/${id}`);
    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      throw new ApiError(error.message);
    }
    throw error;
  }
}

// ✅ Правильно - обработка ошибок
if (error instanceof ApiError) {
  showErrorMessage(error.message);
} else {
  logError(error);
}
```

---

## ⚛️ React

### Компоненты

```typescript
// ✅ Правильно - функциональные компоненты
interface ButtonProps {
  text: string;
  onClick: () => void;
  variant?: 'primary' | 'secondary';
  disabled?: boolean;
}

export const Button: React.FC<ButtonProps> = ({ 
  text, 
  onClick, 
  variant = 'primary',
  disabled = false 
}) => {
  return (
    <button
      className={`btn btn-${variant}`}
      onClick={onClick}
      disabled={disabled}
    >
      {text}
    </button>
  );
};

// ✅ Правильно - экспорт по умолчанию для страниц
const UserProfile: React.FC = () => {
  return <div>Profile</div>;
};

export default UserProfile;
```

### Хуки

```typescript
// ✅ Правильно - кастомные хуки
function useLocalStorage<T>(key: string, initialValue: T) {
  const [storedValue, setStoredValue] = useState<T>(() => {
    try {
      const item = window.localStorage.getItem(key);
      return item ? JSON.parse(item) : initialValue;
    } catch (error) {
      return initialValue;
    }
  });

  const setValue = (value: T | ((val: T) => T)) => {
    try {
      const valueToStore = value instanceof Function ? value(storedValue) : value;
      setStoredValue(valueToStore);
      window.localStorage.setItem(key, JSON.stringify(valueToStore));
    } catch (error) {
      console.log(error);
    }
  };

  return [storedValue, setValue] as const;
}

// ✅ Правильно - зависимости useEffect
useEffect(() => {
  fetchUser(userId);
}, [userId]); // Только нужные зависимости
```

### Состояние

```typescript
// ✅ Правильно - разделение состояния
const [user, setUser] = useState<User | null>(null);
const [isLoading, setIsLoading] = useState(false);
const [error, setError] = useState<string | null>(null);

// ✅ Правильно - useReducer для сложного состояния
interface State {
  data: User[];
  loading: boolean;
  error: string | null;
  page: number;
}

type Action =
  | { type: 'FETCH_START' }
  | { type: 'FETCH_SUCCESS'; payload: User[] }
  | { type: 'FETCH_ERROR'; payload: string }
  | { type: 'SET_PAGE'; payload: number };

const reducer = (state: State, action: Action): State => {
  switch (action.type) {
    case 'FETCH_START':
      return { ...state, loading: true, error: null };
    case 'FETCH_SUCCESS':
      return { ...state, loading: false, data: action.payload };
    case 'FETCH_ERROR':
      return { ...state, loading: false, error: action.payload };
    case 'SET_PAGE':
      return { ...state, page: action.payload };
    default:
      return state;
  }
};
```

---

## 🐘 SQL

### Форматирование

```sql
-- ✅ Правильно - ключевые слова заглавными
SELECT 
    u.id,
    u.email,
    u.created_at,
    COUNT(o.id) as order_count
FROM users u
LEFT JOIN orders o ON o.user_id = u.id
WHERE u.is_active = true
    AND u.created_at >= '2024-01-01'
GROUP BY u.id, u.email, u.created_at
HAVING COUNT(o.id) > 0
ORDER BY u.created_at DESC
LIMIT 100;

-- Отступы для вложенных запросов
SELECT *
FROM (
    SELECT *, ROW_NUMBER() OVER (PARTITION BY user_id ORDER BY created_at DESC) as rn
    FROM orders
) t
WHERE rn = 1;
```

### Именование

```sql
-- ✅ Правильно
CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    email VARCHAR(255) UNIQUE NOT NULL,
    full_name VARCHAR(100),
    created_at TIMESTAMP DEFAULT NOW()
);

CREATE INDEX idx_users_email ON users(email);
CREATE INDEX idx_users_created_at ON users(created_at);

-- Внешние ключи
ALTER TABLE orders 
ADD CONSTRAINT fk_orders_users 
FOREIGN KEY (user_id) REFERENCES users(id);
```

### Производительность

```sql
-- ✅ Всегда используйте EXPLAIN
EXPLAIN ANALYZE
SELECT * FROM orders WHERE user_id = 123;

-- ✅ Используйте индексы для поиска
CREATE INDEX CONCURRENTLY idx_orders_user_id ON orders(user_id);

-- ✅ Избегайте SELECT *
SELECT id, email, full_name FROM users;  -- только нужные поля
```

---

## 🧪 Тестирование

### C# (xUnit)

```csharp
public class PaymentServiceTests
{
    private readonly PaymentService _service;
    private readonly Mock<IPaymentRepository> _repositoryMock;
    
    public PaymentServiceTests()
    {
        _repositoryMock = new Mock<IPaymentRepository>();
        _service = new PaymentService(_repositoryMock.Object);
    }
    
    [Fact]
    public async Task ProcessPayment_ValidRequest_ReturnsSuccess()
    {
        // Arrange
        var request = new PaymentRequest
        {
            Amount = 100,
            Currency = "USD",
            UserId = 1
        };
        
        _repositoryMock
            .Setup(r => r.SaveAsync(It.IsAny<Payment>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await _service.ProcessPaymentAsync(request);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.PaymentId);
        _repositoryMock.Verify(r => r.SaveAsync(It.IsAny<Payment>()), Times.Once);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    public void ProcessPayment_InvalidAmount_ThrowsException(decimal amount)
    {
        // Arrange
        var request = new PaymentRequest { Amount = amount };
        
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _service.ProcessPaymentAsync(request));
    }
}
```

### TypeScript (Jest)

```typescript
import { PaymentService } from './payment.service';
import { ApiClient } from './api.client';

jest.mock('./api.client');

describe('PaymentService', () => {
  let service: PaymentService;
  let apiClient: jest.Mocked<ApiClient>;
  
  beforeEach(() => {
    apiClient = new ApiClient() as jest.Mocked<ApiClient>;
    service = new PaymentService(apiClient);
  });
  
  it('should process payment successfully', async () => {
    // Arrange
    const request = { amount: 100, currency: 'USD' };
    const expectedResponse = { id: '123', status: 'success' };
    
    apiClient.post.mockResolvedValue(expectedResponse);
    
    // Act
    const result = await service.processPayment(request);
    
    // Assert
    expect(result).toEqual(expectedResponse);
    expect(apiClient.post).toHaveBeenCalledWith('/payments', request);
  });
  
  it('should handle network errors', async () => {
    // Arrange
    const request = { amount: 100, currency: 'USD' };
    apiClient.post.mockRejectedValue(new Error('Network error'));
    
    // Act & Assert
    await expect(service.processPayment(request)).rejects.toThrow('Payment failed');
  });
});
```

### Именование тестов

```typescript
// ✅ Правильно - Method_Scenario_ExpectedResult
describe('PaymentService', () => {
  it('processPayment_WithValidRequest_ReturnsSuccess', () => {});
  it('processPayment_WithInvalidAmount_ThrowsException', () => {});
  it('processPayment_WhenApiFails_ReturnsError', () => {});
});

// Или BDD стиль
describe('PaymentService', () => {
  describe('processPayment', () => {
    it('should return success for valid request', () => {});
    it('should throw error for invalid amount', () => {});
    it('should handle API failures gracefully', () => {});
  });
});
```

---

## 📚 Документирование

### C# XML Comments

```csharp
/// <summary>
/// Processes a payment request
/// </summary>
/// <param name="request">The payment request details</param>
/// <param name="cancellationToken">Cancellation token</param>
/// <returns>The payment result with transaction ID</returns>
/// <exception cref="ArgumentException">Thrown when request is invalid</exception>
/// <exception cref="PaymentException">Thrown when payment processing fails</exception>
/// <example>
/// <code>
/// var request = new PaymentRequest { Amount = 100, Currency = "USD" };
/// var result = await service.ProcessPaymentAsync(request);
/// </code>
/// </example>
public async Task<PaymentResult> ProcessPaymentAsync(
    PaymentRequest request,
    CancellationToken cancellationToken = default)
{
    // ...
}
```

### TypeScript JSDoc

```typescript
/**
 * Processes a payment request
 * 
 * @param request - The payment request details
 * @returns Promise with payment result
 * @throws {ValidationError} When request validation fails
 * @throws {ApiError} When API call fails
 * 
 * @example
 * ```ts
 * const result = await paymentService.processPayment({
 *   amount: 100,
 *   currency: 'USD'
 * });
 * console.log(result.transactionId);
 * ```
 */
async function processPayment(request: PaymentRequest): Promise<PaymentResult> {
  // ...
}
```

---

## 🔒 Безопасность

### Общие правила

```csharp
// ✅ Никогда не хранить sensitive данные в коде
// ❌ Плохо
const string connectionString = "Server=localhost;Database=prod;User=sa;Password=admin123;";

// ✅ Хорошо - использовать переменные окружения
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

// ✅ Валидация входных данных
public IActionResult CreateUser(UserDto user)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }
    // ...
}
```

### SQL Injection

```csharp
// ❌ Плохо - уязвимо для SQL injection
var query = $"SELECT * FROM users WHERE email = '{email}'";

// ✅ Хорошо - параметризованные запросы
var user = await _dbContext.Users
    .FirstOrDefaultAsync(u => u.Email == email);

// Для сырых запросов
var user = await _dbContext.Users
    .FromSqlRaw("SELECT * FROM users WHERE email = {0}", email)
    .FirstOrDefaultAsync();
```

### XSS Protection

```typescript
// ❌ Плохо - опасный HTML
function Comment({ text }) {
  return <div dangerouslySetInnerHTML={{ __html: text }} />;
}

// ✅ Хорошо - React автоматически экранирует
function Comment({ text }) {
  return <div>{text}</div>;
}
```

---

## ⚡ Производительность

### C#

```csharp
// ✅ Использовать асинхронные методы
public async Task<List<User>> GetUsersAsync()
{
    return await _dbContext.Users.ToListAsync();
}

// ✅ Использовать Any() вместо Count() > 0
if (users.Any())  // Быстрее чем users.Count() > 0
{
    // ...
}

// ✅ Использовать StringBuilder для конкатенации
var sb = new StringBuilder();
foreach (var item in items)
{
    sb.Append(item);
}
```

### React

```typescript
// ✅ Мемоизация
const expensiveValue = useMemo(() => 
  computeExpensiveValue(a, b), [a, b]
);

const handleClick = useCallback(() => {
  doSomething(a, b);
}, [a, b]);

// ✅ Ленивая загрузка компонентов
const HeavyComponent = React.lazy(() => import('./HeavyComponent'));

// ✅ Виртуализация длинных списков
import { FixedSizeList } from 'react-window';
```

---

## 🛠️ Инструменты

### Обязательные

```json
{
  "scripts": {
    "lint": "eslint src --ext .ts,.tsx",
    "format": "prettier --write \"src/**/*.{ts,tsx}\"",
    "test": "jest",
    "build": "tsc"
  },
  "husky": {
    "hooks": {
      "pre-commit": "lint-staged",
      "pre-push": "npm test"
    }
  },
  "lint-staged": {
    "*.{ts,tsx}": ["eslint --fix", "prettier --write"],
    "*.{json,md}": ["prettier --write"]
  }
}

---

## ✅ Чек-лист перед код-ревью

### Для C#
- [ ] Код отформатирован (Ctrl+K, Ctrl+D)
- [ ] Нет лишних using
- [ ] Асинхронные методы имеют суффикс Async
- [ ] Методы не длиннее 50 строк
- [ ] Классы не длиннее 500 строк
- [ ] Есть XML комментарии для public API
- [ ] Обработаны все исключения
- [ ] Есть unit-тесты

### Для TypeScript/React
- [ ] Код отформатирован (Prettier)
- [ ] Нет ошибок ESLint
- [ ] Правильные типы (никакого `any`)
- [ ] Компоненты не слишком большие
- [ ] Правильные зависимости в хуках
- [ ] Есть тесты для логики
- [ ] Компоненты переиспользуемы

### Для SQL
- [ ] Запрос отформатирован
- [ ] Использованы индексы
- [ ] Нет SELECT *
- [ ] EXPLAIN ANALYZE выполнен
- [ ] Имена соответствуют стандартам

