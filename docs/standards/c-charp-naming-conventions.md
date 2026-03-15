# Соглашения по именованию C#

Правила именования применяются из [src/.editorconfig](src/.editorconfig) и проверяются анализатором (для перечисленных ниже сущностей — severity: error).

| Сущность | Стиль | Правило EditorConfig |
|----------|--------|----------------------|
| Интерфейсы | I + PascalCase | interface_should_be_begins_with_i |
| Типы (class, struct, enum) | PascalCase | types_should_be_pascal_case |
| Методы, свойства, события | PascalCase | non_field_members_should_be_pascal_case |
| Константы (поля) | PascalCase | const_field_should_be_pascal_case |
| Поля (private/internal/private protected) | _ + camelCase | private_or_internal_field_should_be_begins_with__ |
| Параметры методов | camelCase | param_should_be_camel_case |
| Локальные переменные | camelCase | variable_should_be_camel_case |
| Параметры типа (generics) | T + PascalCase | type_param_should_be_begins_with_t |

---

## Пространства имен (Namespaces)
// ✅ ПРАВИЛЬНО - AI, запомни эти паттерны
namespace Company.Project.Module;                    // Иерархия: Компания -> Проект -> Модуль
namespace Company.Project.Shared;                     // Общие компоненты
namespace Company.Project.Services.Payment;           // Сервисы по доменам

// ❌ НЕПРАВИЛЬНО - избегай этих паттернов
namespace Project;                                    // Слишком обще
namespace CompanyProjectModule;                       // Слипшиеся слова
namespace Helpers;   

## Классы
// ✅ ПРАВИЛЬНО - существительные, PascalCase, с суффиксом по назначению
public class UserService        // Сервис для работы с пользователями
public class PaymentProcessor   // Обработчик платежей
public class OrderRepository    // Репозиторий для заказов
public class JwtTokenGenerator  // Генератор токенов
public class EmailValidator     // Валидатор email

// ❌ НЕПРАВИЛЬНО
public class userService         // Не с большой буквы
public class ProcessPayment      // Глагол, не существительное
public class Util                // Слишком абстрактно
public class MyClass             // Бессмысленно
// Проверяется в .editorconfig (error).

## Интерфейсы
// ✅ ПРАВИЛЬНО - I + PascalCase, отражает контракт
public interface IUserRepository      // Контракт репозитория
public interface IPaymentService      // Контракт сервиса
public interface IJwtProvider         // Поставщик JWT
public interface IEmailSender         // Отправитель email

// ❌ НЕПРАВИЛЬНО
public interface UserRepository        // Нет префикса I
public interface IPayment              // Слишком коротко
public interface I                     // Одна буква
// Проверяется в .editorconfig (error).

## Методы
// ✅ ПРАВИЛЬНО - глагол + что делаем, PascalCase
public async Task<User> GetUserByIdAsync(int id)           // Получение
public void ProcessPayment(PaymentRequest request)         // Обработка
public bool ValidateToken(string token)                    // Проверка
public Task SaveChangesAsync(CancellationToken ct)        // Сохранение
private string GenerateHash(string input)                  // Генерация

// ❌ НЕПРАВИЛЬНО
public async Task<User> getUserById(int id)                // Не с большой
public void process_payment(PaymentRequest request)        // Не snake_case
public bool Validate(string str)                           // Слишком общо
// Проверяется в .editorconfig (error).

## Параметры методов
// ✅ ПРАВИЛЬНО - camelCase
GetUserById(int id)
ProcessPayment(PaymentRequest request)
SaveChangesAsync(CancellationToken cancellationToken)

// ❌ НЕПРАВИЛЬНО
GetUserById(int Id)                        // Не PascalCase для параметра
ProcessPayment(PaymentRequest user_request) // Не snake_case
// Проверяется в .editorconfig (error).

## Правила для префиксов методов
// AI, используй эти префиксы последовательно:

// CRUD операции
GetUser()           // Получение одного
GetUsers()          // Получение множества
CreateUser()        // Создание
UpdateUser()        // Обновление
DeleteUser()        // Удаление

// Проверки (возвращают bool)
IsValid()           // Валидность
Exists()            // Существование
HasPermission()     // Наличие прав
CanDelete()         // Возможность

// Преобразования
ToDto()             // В DTO
FromEntity()        // Из сущности
MapToModel()        // Маппинг

// Асинхронность (всегда с Async суффиксом). Проверяется: RCS1046 (suggestion), RCS1047 (error).
GetUserAsync()
SaveChangesAsync()
ProcessAsync()

## Свойства
// ✅ ПРАВИЛЬНО - существительные, PascalCase
public int Id { get; set; }                    // Идентификатор
public string UserName { get; set; }            // Имя пользователя
public DateTime CreatedAt { get; private set; } // Дата создания
public ICollection<Order> Orders { get; set; }  // Коллекция заказов
public bool IsActive { get; set; }              // Флаг активности

// ❌ НЕПРАВИЛЬНО
public int id { get; set; }                      // Не с маленькой
public string user_name { get; set; }            // Не snake_case
public ICollection<Order> _orders { get; set; }  // С подчеркиванием
// Проверяется в .editorconfig (error).

## Поля (private / internal / private protected)
// ✅ ПРАВИЛЬНО - _ + camelCase для полей с доступом private, internal или private protected
private readonly IUserRepository _userRepository;
private readonly string _connectionString;
internal readonly ILogger _logger;
private protected int _retryCount;

// ❌ НЕПРАВИЛЬНО
private IUserRepository userRepository;          // Без подчеркивания
private string ConnectionString;                  // PascalCase
private int m_retryCount;                         // Венгерская нотация
// Проверяется в .editorconfig (error).

## Константы
// ✅ ПРАВИЛЬНО - PascalCase
public const int DefaultPageSize = 20;
public const string ConnectionStringName = "DefaultConnection";
private const int MaxRetryCount = 3;

// ❌ НЕПРАВИЛЬНО
public const int DEFAULT_PAGE_SIZE = 20;          // Капс в C# не принят
private const string _connectionStringName;        // С подчеркиванием
// Проверяется в .editorconfig (error).

## Локальные переменные
// ✅ ПРАВИЛЬНО - camelCase
int userCount = 0;
var result = await GetUserAsync(id);
string connectionString = GetConnectionString();

// ❌ НЕПРАВИЛЬНО
int UserCount = 0;                  // Не PascalCase
var user_count = GetUser();         // Не snake_case
// Проверяется в .editorconfig (error).

## Параметры типа (generics)
// ✅ ПРАВИЛЬНО - префикс T, далее PascalCase
public class Repository<T>
public class KeyValuePair<TKey, TValue>
public interface IHandler<TRequest, TResponse>

// ❌ НЕПРАВИЛЬНО
public class Repository<Key>           // Без префикса T
public class KeyValuePair<tKey, tValue> // T должно быть заглавной
// Проверяется в .editorconfig (error).

## Enum
// ✅ ПРАВИЛЬНО - PascalCase для типа, PascalCase для значений
public enum OrderStatus
{
    Pending,        // Ожидает
    Paid,           // Оплачен
    Shipped,        // Отправлен
    Delivered,      // Доставлен
    Cancelled       // Отменен
}

public enum ErrorCode
{
    NotFound = 404,
    BadRequest = 400,
    Unauthorized = 401
}

// ❌ НЕПРАВИЛЬНО
public enum orderStatus                            // Не с большой
{
    PENDING,            // Капс не нужен
    paid,               // Разнобой
    order_status_3      // Непонятно
}
// Проверяется в .editorconfig (error).

## События
// ✅ ПРАВИЛЬНО - PascalCase (как методы и свойства)
public event EventHandler OrderCreated;
public event EventHandler<OrderEventArgs> StatusChanged;

// ❌ НЕПРАВИЛЬНО
public event EventHandler order_created;  // Не snake_case
public event EventHandler orderCreated;   // Не camelCase — события в PascalCase
// Проверяется в .editorconfig (error).

## DTO (Data Transfer Objects)
// ✅ ПРАВИЛЬНО - суффикс Request/Response/Dto
public class CreateUserRequest      // Запрос на создание
public class UserResponse           // Ответ с данными пользователя
public class OrderDto               // DTO для заказа
public class UpdateProfileCommand   // Команда на обновление

// ❌ НЕПРАВИЛЬНО
public class User                   // Путается с Entity
public class CreateUser             // Непонятно, это Request или Response