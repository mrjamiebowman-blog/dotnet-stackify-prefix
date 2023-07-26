using Microsoft.Extensions.DependencyInjection;
using MrJB.OTel.Prefix.Data.Services;

namespace MrJB.OTel.Prefix.Data;

public static class Builder
{
    public static IServiceCollection AddCustomDataService(this IServiceCollection services)
    {
        // inject
        services.AddTransient<IDataService, DataService>();

        return services;
    }
}
