using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace GM.Shared.Observability.Extensions;

/// <summary>
/// Общие расширения для подключения OpenTelemetry в сервисах системы.
/// </summary>
public static class ObservabilityExtensions
{
    /// <summary>
    /// Подключает единый baseline мониторинга: OTLP-экспорт логов, трейсов и метрик.
    /// </summary>
    /// <param name="services">DI-контейнер сервиса.</param>
    /// <param name="logging">Конфигуратор логирования приложения.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <param name="serviceName">Логическое имя сервиса в телеметрии.</param>
    /// <param name="serviceVersion">Версия сервиса в телеметрии.</param>
    /// <returns>DI-контейнер для цепочки вызовов.</returns>
    public static IServiceCollection AddGmObservability(
        this IServiceCollection services,
        ILoggingBuilder logging,
        IConfiguration configuration,
        string serviceName,
        string serviceVersion)
    {
        var otlpEndpoint = configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "http://localhost:4317";
        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(serviceName: serviceName, serviceVersion: serviceVersion);

        logging.AddOpenTelemetry(options =>
        {
            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;
            options.ParseStateValues = true;
            options.SetResourceBuilder(resourceBuilder);
            options.AddOtlpExporter(exporter => exporter.Endpoint = new Uri(otlpEndpoint));
        });

        services.AddOpenTelemetry()
            .ConfigureResource(resource =>
                resource.AddService(serviceName: serviceName, serviceVersion: serviceVersion))
            .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter(exporter => exporter.Endpoint = new Uri(otlpEndpoint)))
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddOtlpExporter(exporter => exporter.Endpoint = new Uri(otlpEndpoint)));

        return services;
    }
}
