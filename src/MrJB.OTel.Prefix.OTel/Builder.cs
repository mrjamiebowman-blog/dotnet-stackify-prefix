using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace MrJB.OTel.Prefix.OTel;

public static class Builder
{
    public static TracerProviderBuilder AddAppTracing(this TracerProviderBuilder builder, IServiceProvider services, ResourceBuilder? resource = null)
    {
        resource ??= ResourceBuilder.CreateDefault()
            .AddTelemetrySdk()
            .AddEnvironmentVariableDetector();
        var tracesExporter = services
        .GetRequiredService<IConfiguration>()
        .GetSection("OpenTelemetry:Traces");
        builder
        .AddSource(OTel.Application.Name) // , App.RabbitMq.Name, App.Redis.Name, "Npgsql"
            .AddHttpClientInstrumentation() // nuget: OpenTelemetry.Instrumentation.Http (pre-release)
            /*
                new OtlpExporterOptions
                {
                    Endpoint = null, // uri
                    Headers = null, // string
                    TimeoutMilliseconds = 0,
                    Protocol = OtlpExportProtocol.Grpc,
                    ExportProcessorType = ExportProcessorType.Simple,
                    BatchExportProcessorOptions = new BatchExportActivityProcessorOptions
                    {
                        MaxQueueSize = 0,
                        ScheduledDelayMilliseconds = 0,
                        ExporterTimeoutMilliseconds = 0,
                        MaxExportBatchSize = 0
                    },
                }
            */
            .AddOtlpExporter(opts => tracesExporter.Bind(opts))
            .AddConsoleExporter();
        return builder;
    }
}