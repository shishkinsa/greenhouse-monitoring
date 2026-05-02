namespace GM.WebApi.UseCases.Handlers.Region.Commands.CreateRegion.Requests;

/// <summary>
/// Объект запроса для создания региона
/// </summary>
public class CreateRegionRequest
{
    /// <summary>
    /// Уникальный код региона
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Наименование региона
    /// </summary>
    public string Name { get; set; }
}
