using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.Location.Commands.DeleteLocation;

public sealed class DeleteLocationCommand : ICommand
{
    public Guid LocationId { get; set; }
}
