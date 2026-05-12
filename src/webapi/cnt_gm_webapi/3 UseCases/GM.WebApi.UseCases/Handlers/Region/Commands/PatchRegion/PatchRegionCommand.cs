using GM.WebApi.UseCases.Handlers.Region.DTOs;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.Region.Commands.PatchRegion;

/// <summary>
/// Частичное обновление региона по идентификатору.
/// </summary>
public sealed class PatchRegionCommand : ICommand<RegionDto>
{
    public Guid RegionId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public bool? IsActive { get; set; }
}
