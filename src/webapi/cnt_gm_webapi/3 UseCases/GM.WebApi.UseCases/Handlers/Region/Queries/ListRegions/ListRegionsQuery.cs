using GM.WebApi.UseCases.Handlers.Region.Queries.ListRegions.Responses;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.Region.Queries.ListRegions;

public class ListRegionsQuery : IQuery<RegionListResponse>
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 25;
}
