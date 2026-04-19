using GM.WebApi.Entities.Models;
using GM.WebApi.Infrastructure.Interfaces.DataAccess;

using Microsoft.EntityFrameworkCore;

namespace GM.WebApi.DataAccess.Postgres.Data;

public class AppDbContext : DbContext, IDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Organization> Organizations { get; set; }

    public DbSet<UserOrganizationMembership> UserOrganizationMemberships { get; set; }

    public DbSet<Region> Regions { get; set; }

    public DbSet<Location> Locations { get; set; }

    public DbSet<Greenhouse> Greenhouses { get; set; }

    public DbSet<SensorType> SensorTypes { get; set; }

    public DbSet<Sensor> Sensors { get; set; }

    public DbSet<DigitalController> DigitalControllers { get; set; }

    public DbSet<VideoCamera> VideoCameras { get; set; }

    public DbSet<GreenhouseDigitalControllerInstallation> GreenhouseDigitalControllerInstallations { get; set; }

    public DbSet<SensorDigitalControllerLink> SensorDigitalControllerLinks { get; set; }

    public DbSet<GreenhouseVideoCameraInstallation> GreenhouseVideoCameraInstallations { get; set; }

    public DbSet<SensorThresholdRule> SensorThresholdRules { get; set; }
}
