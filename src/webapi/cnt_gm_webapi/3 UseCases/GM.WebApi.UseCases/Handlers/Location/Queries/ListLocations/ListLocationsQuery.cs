using GM.WebApi.UseCases.Handlers.Location.Queries.ListLocations.Responses;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.Location.Queries.ListLocations;

public sealed class ListLocationsQuery : IQuery<LocationListResponse>
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 25;
}
