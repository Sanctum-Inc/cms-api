using System.Reflection;
using Mapster;
using MapsterMapper;

namespace Api.Configuration;

public static class MapsterConfiguration
{
    public static IServiceCollection AddMapsterMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly()); // Scans for IRegister implementations

        var mapperConfig = new Mapper(config);
        services.AddSingleton<IMapper>(mapperConfig); // Registers IMapper for DI

        return services;
    }
}
