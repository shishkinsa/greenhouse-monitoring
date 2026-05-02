using System;
using System.Collections.Generic;
using System.Text;

namespace GM.WebApi.Utils.DTOs;

/// <summary>
/// Информация о странице для ответа на запросы с пагинацией
/// </summary>
public class PageInfo
{
    /// <summary>Номер страницы</summary>
    public int Page { get; set; }

    /// <summary>Размер страницы</summary>
    public int PageSize { get; set; }
    
    /// <summary>Общее количество элементов</summary>
    public int TotalItems { get; set; }
}
