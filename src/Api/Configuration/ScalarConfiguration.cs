using Scalar.AspNetCore;

namespace Api.Configuration;

public static class ScalarConfiguration
{
    public static void ConfigureServices(IEndpointRouteBuilder endpoints) // Change parameter type to IEndpointRouteBuilder
    {
        endpoints.MapScalarApiReference(options =>
        {
            options.WithTitle("Case Management System API")
                   .WithDarkModeToggle()
                   .WithSidebar();
        });
    }
}
