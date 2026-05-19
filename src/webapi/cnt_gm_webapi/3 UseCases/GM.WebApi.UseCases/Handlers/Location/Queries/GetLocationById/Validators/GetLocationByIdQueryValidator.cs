using FluentValidation;

namespace GM.WebApi.UseCases.Handlers.Location.Queries.GetLocationById.Validators;

public sealed class GetLocationByIdQueryValidator : AbstractValidator<GetLocationByIdQuery>
{
    public GetLocationByIdQueryValidator()
    {
        RuleFor(x => x.LocationId)
            .NotEqual(Guid.Empty)
            .WithMessage("Идентификатор локации не должен быть пустым.");
    }
}
