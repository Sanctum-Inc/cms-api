using Scalar.AspNetCore;

namespace cms_api.Configuration;

public static class ScalarConfiguration
{
    public static void AddScalar(IEndpointRouteBuilder endpoints) // Change parameter type to IEndpointRouteBuilder
    {
        endpoints.MapScalarApiReference(options =>
        {
            options.WithTitle("E-Commerce API")
                   .AddServer("https://api.company.com", "Production")
                   .AddServer("https://staging-api.company.com", "Staging")
                   .WithDarkModeToggle()
                   .WithSidebar();
        });
    }
}
