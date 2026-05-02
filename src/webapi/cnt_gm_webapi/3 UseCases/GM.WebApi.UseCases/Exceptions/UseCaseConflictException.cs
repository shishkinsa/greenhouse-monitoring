namespace GM.WebApi.UseCases.Exceptions;

/// <summary>
/// Конфликт бизнес-правил (например уникальность кода) — маппится на HTTP <c>409</c>.
/// </summary>
public sealed class UseCaseConflictException : Exception
{
    public UseCaseConflictException(string message)
        : base(message)
    {
    }
}
