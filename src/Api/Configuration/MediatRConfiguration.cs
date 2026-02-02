using Application;
using Application.Common.Behaviours;
using FluentValidation;
using MediatR;

namespace Api.Configuration;

public static class MediatRConfiguration
{
    public static IServiceCollection AddMediatR(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
        {
            cfg.LicenseKey = configuration.GetSection("Mediatr:License").Value;
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly);
        });

        // ✅ Register all validators in Application assembly at once
        services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);

        // ✅ Add validation behavior globally
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
