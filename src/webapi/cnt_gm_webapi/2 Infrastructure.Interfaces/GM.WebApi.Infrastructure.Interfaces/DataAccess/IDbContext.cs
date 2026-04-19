using GM.WebApi.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace GM.WebApi.Infrastructure.Interfaces.DataAccess;

/// <summary>
/// Абстрактный доступ к данным для слоя UseCases.
/// </summary>
public interface IDbContext
{
    DbSet<Organization> Organizations { get; set; }

    DbSet<UserOrganizationMembership> UserOrganizationMemberships { get; set; }

    DbSet<Region> Regions { get; set; }

    DbSet<Location> Locations { get; set; }

    DbSet<Greenhouse> Greenhouses { get; set; }

    DbSet<SensorType> SensorTypes { get; set; }

    DbSet<Sensor> Sensors { get; set; }

    DbSet<DigitalController> DigitalControllers { get; set; }

    DbSet<VideoCamera> VideoCameras { get; set; }

    DbSet<GreenhouseDigitalControllerInstallation> GreenhouseDigitalControllerInstallations { get; set; }

    DbSet<SensorDigitalControllerLink> SensorDigitalControllerLinks { get; set; }

    DbSet<GreenhouseVideoCameraInstallation> GreenhouseVideoCameraInstallations { get; set; }

    DbSet<SensorThresholdRule> SensorThresholdRules { get; set; }

    int SaveChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
