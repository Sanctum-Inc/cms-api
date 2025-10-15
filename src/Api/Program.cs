// Ensure the Scalar namespace is correctly referenced and the AddScalar method exists.
// If the Scalar.AspNetCore package is missing, install it via NuGet Package Manager or CLI:
// dotnet add package Scalar.AspNetCore

using Api.Configuration;
using FluentValidation;
using Infrastructure;
using Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication(builder.Configuration);

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddMediatR(builder.Configuration);

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Add services to the container.
builder.Services.AddControllers();


builder.Services.AddOpenApiWithAuth();

builder.Services.AddMapsterMappings();

builder.Services.AddHttpContextAccessor();

builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

app.ExecutePendingMigrations();

// Configure the HTTP request pipeline.

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapOpenApi();

ScalarConfiguration.ConfigureServices(app);

app.Run();
