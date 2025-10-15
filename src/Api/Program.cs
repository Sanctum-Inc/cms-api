// Ensure the Scalar namespace is correctly referenced and the AddScalar method exists.
// If the Scalar.AspNetCore package is missing, install it via NuGet Package Manager or CLI:
// dotnet add package Scalar.AspNetCore

using Api.Configuration;
using FluentValidation;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

DependecyInjection.AddInfrastructure(builder.Services, builder.Configuration);

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddMediatR(builder.Configuration);

builder.Services.AddValidatorsFromAssemblyContaining<Program>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApiWithAuth();

builder.Services.AddMapsterMappings();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(builder.Configuration);

var app = builder.Build();

app.ExecutePendingMigrations();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    ScalarConfiguration.ConfigureServices(app);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();


app.Run();
