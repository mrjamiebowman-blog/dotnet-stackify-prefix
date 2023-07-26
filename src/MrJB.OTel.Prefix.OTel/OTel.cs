using System.Diagnostics;

namespace MrJB.OTel.Prefix.OTel;

/// <summary>
/// OTel Constants & Activity Sources
/// </summary>
public static class OTel
{
    public static string ServiceName { get; set; } = "MrJB.Otel.Prefix";

    public static string ServiceVersion { get; set; } = "1.0.0";

    public static readonly ActivitySource Application = new(ServiceName);

    //public static readonly ActivitySource RabbitMq = new(nameof(RabbitMq));

    //public static readonly ActivitySource Redis = new("OpenTelemetry.Instrumentation.StackExchangeRedis");
}
