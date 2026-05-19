using FluentValidation;

namespace GM.WebApi.UseCases.Handlers.Region.Queries.GetRegionById.Validators;

public sealed class GetRegionByIdQueryValidator : AbstractValidator<GetRegionByIdQuery>
{
    public GetRegionByIdQueryValidator()
    {
        RuleFor(x => x.RegionId)
            .NotEqual(Guid.Empty)
            .WithMessage("Идентификатор региона не должен быть пустым.");
    }
}
