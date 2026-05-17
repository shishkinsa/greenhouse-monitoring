using GM.WebApi.UseCases.Handlers.SensorTypes.Queries.ListSensorTypes.Responses;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.SensorTypes.Queries.ListSensorTypes;

public class ListSensorTypesQuery : IQuery<SensorTypeListResponse>
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 25;
}
