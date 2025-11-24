using Mapster;
using MapsterMapper;
using System.Reflection;
using Scalar.AspNetCore;

namespace Api.Configuration;

public static class CorsConfiguration
{
    public static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowUI", policy =>
            {
                policy
                    .SetIsOriginAllowed(_ => true)  // allow ANY origin
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });

        });

        return services;
    }
}
