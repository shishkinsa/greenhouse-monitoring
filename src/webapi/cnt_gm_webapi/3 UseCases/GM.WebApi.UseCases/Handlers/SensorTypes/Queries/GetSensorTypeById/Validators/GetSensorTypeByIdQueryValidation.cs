using FluentValidation;

using GM.WebApi.UseCases.Handlers.SensorTypes.Queries.GetSensorTypeById;

namespace GM.WebApi.UseCases.Handlers.SensorTypes.Queries.GetSensorTypeById.Validators;

/// <summary>
/// Валидация запроса типа датчика по идентификатору.
/// </summary>
public sealed class GetSensorTypeByIdQueryValidation : AbstractValidator<GetSensorTypeByIdQuery>
{
    public GetSensorTypeByIdQueryValidation()
    {
        RuleFor(x => x.SensorTypeId)
            .NotEqual(Guid.Empty)
            .WithMessage("Идентификатор типа датчика не должен быть пустым.");
    }
}
