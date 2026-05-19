namespace GM.WebApi.UseCases.Handlers.Location.Commands.CreateLocation.Requests;

public sealed class CreateLocationRequest
{
    public Guid RegionId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Address { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }
}
