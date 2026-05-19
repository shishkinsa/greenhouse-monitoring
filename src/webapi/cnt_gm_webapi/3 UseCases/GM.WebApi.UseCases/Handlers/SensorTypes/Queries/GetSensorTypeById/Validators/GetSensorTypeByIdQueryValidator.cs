using FluentValidation;

namespace GM.WebApi.UseCases.Handlers.SensorTypes.Queries.GetSensorTypeById.Validators;

/// <summary>
/// Валидация запроса типа датчика по идентификатору.
/// </summary>
public sealed class GetSensorTypeByIdQueryValidator : AbstractValidator<GetSensorTypeByIdQuery>
{
    public GetSensorTypeByIdQueryValidator()
    {
        RuleFor(x => x.SensorTypeId)
            .NotEqual(Guid.Empty)
            .WithMessage("Идентификатор типа датчика не должен быть пустым.");
    }
}
