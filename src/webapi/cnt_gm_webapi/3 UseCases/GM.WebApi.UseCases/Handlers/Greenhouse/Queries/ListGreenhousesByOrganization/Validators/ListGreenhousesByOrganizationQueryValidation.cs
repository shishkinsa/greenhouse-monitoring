using FluentValidation;

using GM.WebApi.UseCases.Handlers.Greenhouse.Queries.ListGreenhousesByOrganization;

namespace GM.WebApi.UseCases.Handlers.Greenhouse.Queries.ListGreenhousesByOrganization.Validators;

public sealed class ListGreenhousesByOrganizationQueryValidation : AbstractValidator<ListGreenhousesByOrganizationQuery>
{
    public ListGreenhousesByOrganizationQueryValidation()
    {
        RuleFor(x => x.OrganizationId)
            .NotEqual(Guid.Empty)
            .WithMessage("Идентификатор организации не должен быть пустым.");
    }
}
