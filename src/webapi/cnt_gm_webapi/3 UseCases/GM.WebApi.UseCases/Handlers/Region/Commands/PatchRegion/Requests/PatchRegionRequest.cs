namespace GM.WebApi.UseCases.Handlers.Region.Commands.PatchRegion.Requests;

/// <summary>
/// Тело запроса PATCH региона (все поля опциональны; должен быть указан хотя бы один).
/// </summary>
public sealed class PatchRegionRequest
{
    /// <summary>Новый код региона (если передан — заменяет текущий).</summary>
    public string? Code { get; set; }

    /// <summary>Новое наименование (если передано — заменяет текущее).</summary>
    public string? Name { get; set; }

    /// <summary>Новый признак активности (если передан — заменяет текущий).</summary>
    public bool? IsActive { get; set; }
}
