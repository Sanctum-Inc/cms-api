using System.Reflection;
using Mapster;
using MapsterMapper;

namespace Api.Configuration;

public static class MapsterConfiguration
{
    public static IServiceCollection AddMapsterMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;

        // 🔹 CRITICAL FIX: Tell Mapster to map null values, not skip them
        config.Default.IgnoreNullValues(false);

        // Optional: Additional settings for better mapping behavior
        config.Default
            .PreserveReference(true)     // Helps with circular references
            .MapToConstructor(false)     // Use property setters instead of constructors
            .ShallowCopyForSameType(false); // Don't shallow copy, actually map properties

        // Scan for IRegister implementations
        config.Scan(Assembly.GetExecutingAssembly());

        // Compile for better performance
        config.Compile();

        var mapperConfig = new Mapper(config);
        services.AddSingleton<IMapper>(mapperConfig);

        return services;
    }
}
