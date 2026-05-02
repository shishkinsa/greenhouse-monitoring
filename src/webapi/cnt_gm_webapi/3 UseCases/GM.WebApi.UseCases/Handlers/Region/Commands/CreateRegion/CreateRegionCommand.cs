using GM.WebApi.UseCases.Handlers.Region.Commands.CreateRegion.Responses;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.Region.Commands.CreateRegion;

public class CreateRegionCommand : ICommand<CreateRegionResponse>
{
    /// <summary>
    /// Уникальный код региона
    /// </summary>
    public string Code { get; set; } = "";

    /// <summary>
    /// Наименование региона
    /// </summary>
    public string Name { get; set; } = "";
}
