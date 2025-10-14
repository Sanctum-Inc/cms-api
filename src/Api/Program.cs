// Ensure the Scalar namespace is correctly referenced and the AddScalar method exists.
// If the Scalar.AspNetCore package is missing, install it via NuGet Package Manager or CLI:
// dotnet add package Scalar.AspNetCore

using Api.Configuration;
using Scalar.AspNetCore; // Ensure this namespace is correct and the package is installed.

var builder = WebApplication.CreateBuilder(args);

PersistenceConfigurator.ConfigureServices(builder.Services, builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    ScalarConfiguration.ConfigureServices(app);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
