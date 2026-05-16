using GM.WebApi.UseCases.Handlers.Greenhouse.Queries.ListGreenhousesByOrganization.Responses;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.Greenhouse.Queries.ListGreenhousesByOrganization;

/// <summary>Теплицы организации с пагинацией.</summary>
public class ListGreenhousesByOrganizationQuery : IQuery<GreenhouseListResponse>
{
    /// <summary>Идентификатор организации (<c>organizations.id</c>).</summary>
    public Guid OrganizationId { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 25;
}
