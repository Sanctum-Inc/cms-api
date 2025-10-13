using Scalar.AspNetCore;

namespace cms_api.Configuration;

public static class ScalarConfiguration
{
    public static void AddScalar(IEndpointRouteBuilder endpoints) // Change parameter type to IEndpointRouteBuilder
    {
        endpoints.MapScalarApiReference(options =>
        {
            options.WithTitle("Case Management System API")
                   .WithDarkModeToggle()
                   .WithSidebar();
        });
    }
}
