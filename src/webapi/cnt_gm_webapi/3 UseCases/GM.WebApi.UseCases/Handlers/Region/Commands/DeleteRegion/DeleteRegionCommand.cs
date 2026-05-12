using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.Region.Commands.DeleteRegion;

public sealed class DeleteRegionCommand : ICommand
{
    public Guid RegionId { get; set; }
}
