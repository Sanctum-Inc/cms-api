namespace Api.Configuration;

public static class CorsConfiguration
{
    public static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowUI", policy =>
            {
                policy.SetIsOriginAllowed(origin =>
                        origin == "https://lexcase.co.za" ||
                        origin == "https://www.lexcase.co.za" ||
                        origin.StartsWith("https://sanctum-inc.github.io")
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }
}
