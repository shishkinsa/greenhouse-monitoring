using GM.WebApi.UseCases.Handlers.Location.DTOs;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.Location.Queries.GetLocationById;

public sealed class GetLocationByIdQuery : IQuery<LocationDto>
{
    public Guid LocationId { get; set; }
}
