using GM.WebApi.UseCases.Handlers.Region.Queries.GetRegionById.Responses;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.Region.Queries.GetRegionById;

/// <summary>
/// Объект запроса региона по его идентификатору
/// </summary>
public class GetRegionByIdQuery: IQuery<GetRegionByIdResponse>
{
    /// <summary>
    /// Идентификатор региона для его получения
    /// </summary>
    public Guid RegionId { get; set; }
}
