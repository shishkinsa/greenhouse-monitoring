using GM.WebApi.UseCases.Handlers.Location.DTOs;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.Location.Commands.CreateLocation;

public sealed class CreateLocationCommand : ICommand<LocationDto>
{
    public Guid RegionId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Address { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }
}
