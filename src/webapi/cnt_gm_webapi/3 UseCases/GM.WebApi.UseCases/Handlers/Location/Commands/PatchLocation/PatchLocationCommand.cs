using GM.WebApi.UseCases.Handlers.Location.DTOs;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.Location.Commands.PatchLocation;

public sealed class PatchLocationCommand : ICommand<LocationDto>
{
    public Guid LocationId { get; set; }

    public Guid? RegionId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public bool? IsActive { get; set; }
}
