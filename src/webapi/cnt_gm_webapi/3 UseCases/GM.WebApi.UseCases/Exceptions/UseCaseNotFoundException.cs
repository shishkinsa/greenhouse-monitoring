namespace GM.WebApi.UseCases.Exceptions;

/// <summary>
/// Исключение, которое выбрасывается, когда запрос не найден.
/// </summary>
public sealed class UseCaseNotFoundException : Exception
{
    public UseCaseNotFoundException(string message)
        : base(message)
    {
    }
}