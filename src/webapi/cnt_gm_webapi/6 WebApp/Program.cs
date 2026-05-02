using GM.WebApi.DataAccess.Postgres.Data;
using GM.WebApi.Infrastructure.Interfaces.DataAccess;
using GM.WebApi.UseCases.Handlers.Region.Commands.CreateRegion.Validations;
using GM.WebApi.UseCases.Handlers.WeatherForecast.Queries.GetWeather;
using GM.WebApi.WebApp.ExceptionHandlers;
using GM.Shared.Observability.Extensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var serviceName = "GM.WebApi.WebApp";
var serviceVersion = typeof(Program).Assembly.GetName().Version?.ToString() ?? "1.0.0";

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IDbContext>(sp => sp.GetRequiredService<AppDbContext>());
builder.Services.AddValidatorsFromAssemblyContaining<CreateRegionCommandValidator>();
builder.Services.AddRequestum(cfg =>
{
    cfg.RegisterHandlers(typeof(GetWeatherForecastQuery).Assembly);
    cfg.RegisterMiddlewares(typeof(Program).Assembly);
});
builder.Services.AddGmObservability(
    builder.Logging,
    builder.Configuration,
    serviceName,
    serviceVersion);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
