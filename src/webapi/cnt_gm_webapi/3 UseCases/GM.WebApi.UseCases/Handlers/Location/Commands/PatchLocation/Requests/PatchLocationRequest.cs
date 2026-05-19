namespace GM.WebApi.UseCases.Handlers.Location.Commands.PatchLocation.Requests;

public sealed class PatchLocationRequest
{
    public Guid? RegionId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public bool? IsActive { get; set; }
}
