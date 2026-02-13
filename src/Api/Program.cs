// Ensure the Scalar namespace is correctly referenced and the AddScalar method exists.
// If the Scalar.AspNetCore package is missing, install it via NuGet Package Manager or CLI:
// dotnet add package Scalar.AspNetCore

using Api.Configuration;
using Application;
using FluentValidation;
using Infrastructure;
using QuestPDF;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication(builder.Configuration);

var env = builder.Environment.EnvironmentName;
var connString = builder.Configuration.GetConnectionString("DefaultConnection");

Console.WriteLine($"Current Environment: {env}");
Console.WriteLine($"Connection String Found: {!string.IsNullOrEmpty(connString)}");

if (string.IsNullOrEmpty(connString)) {
    throw new Exception($"Connection string is missing! Check appsettings.{env}.json");
}

builder.Services.AddPersistence(builder.Configuration, builder.Environment);
builder.Services.AddMediatR(builder.Configuration);

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Add services to the container.
builder.Services.AddControllers();


builder.Services.AddOpenApiWithAuth();

builder.Services.AddMapsterMappings();

builder.Services.AddHttpContextAccessor();

builder.Services.AddJwtAuthentication(builder.Configuration, builder.Environment);

builder.Services.AddCustomCors();

builder.Services.AddHostedService<Worker.EmailWorker.EmailHandler>();

var app = builder.Build();


app.ExecutePendingMigrations(builder.Environment);

// Configure the HTTP request pipeline.

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors("AllowUI");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapOpenApi();

ScalarConfiguration.ConfigureServices(app);

Settings.License = LicenseType.Community;

await app.RunAsync();

public partial class Program
{
} // 👈 Required for WebApplicationFactory
