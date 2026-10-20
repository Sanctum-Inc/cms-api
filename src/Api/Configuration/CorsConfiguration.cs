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
                        origin == "https://cms-react-rfh8.onrender.com" ||
                        origin.StartsWith("http://localhost") ||
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
