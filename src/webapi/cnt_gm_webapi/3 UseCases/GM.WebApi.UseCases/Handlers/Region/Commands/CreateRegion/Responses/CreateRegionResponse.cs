using System;
using System.Collections.Generic;
using System.Text;

namespace GM.WebApi.UseCases.Handlers.Region.Commands.CreateRegion.Responses;

/// <summary>
/// Объект результата создания региона
/// </summary>
public class CreateRegionResponse
{
    /// <summary>
    /// Уникальный идентификатор региона (UUID)
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Уникальный код региона
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Наименование региона
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Признак активности региона (true — активен)
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Дата и время создания записи
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Дата и время последнего обновления записи
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }
}
