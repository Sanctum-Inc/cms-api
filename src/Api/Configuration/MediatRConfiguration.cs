using Application;

namespace Api.Configuration;

public static class MediatRConfiguration
{
    public static IServiceCollection AddMediatR(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => {
            cfg.LicenseKey = configuration.GetSection("Mediatr:License").Value;
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly);
        });

        return services;
    }
}
