using System.Text.Json;

using FluentValidation;

using GM.WebApi.UseCases.Exceptions;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GM.WebApi.WebApp.ExceptionHandlers;

/// <summary>
/// Обработка доменных исключений сценариев: валидация FluentValidation (<c>400</c>), не найдено (<c>404</c>), конфликт (<c>409</c>).
/// </summary>
public sealed class ApiExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is UseCaseNotFoundException notFound)
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            httpContext.Response.ContentType = "application/problem+json";

            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Ресурс не найден",
                Detail = notFound.Message,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
            };

            await httpContext.Response.WriteAsJsonAsync(
                problem,
                (JsonSerializerOptions?)null,
                "application/problem+json",
                cancellationToken);
            return true;
        }

        if (exception is UseCaseConflictException conflict)
        {
            httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
            httpContext.Response.ContentType = "application/problem+json";

            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Конфликт данных",
                Detail = conflict.Message,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8"
            };

            await httpContext.Response.WriteAsJsonAsync(
                problem,
                (JsonSerializerOptions?)null,
                "application/problem+json",
                cancellationToken);
            return true;
        }

        if (exception is ValidationException validationException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            httpContext.Response.ContentType = "application/problem+json";

            var errors = validationException.Errors
                .GroupBy(e => string.IsNullOrEmpty(e.PropertyName) ? "_" : e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray());

            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Некорректный запрос",
                Detail = "Ошибка валидации входных данных.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            problem.Extensions["errors"] = errors;

            await httpContext.Response.WriteAsJsonAsync(
                problem,
                (JsonSerializerOptions?)null,
                "application/problem+json",
                cancellationToken);
            return true;
        }

        return false;
    }
}
