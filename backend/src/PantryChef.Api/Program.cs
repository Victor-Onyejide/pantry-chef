using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PantryChef.Api.Health;

var builder = WebApplication.CreateBuilder(args);

// Fail fast: refuse to start without a connection string rather than failing on the first request.
var connectionString = builder.Configuration.GetConnectionString("PantryChef")
    ?? throw new InvalidOperationException(
        "Connection string 'PantryChef' is missing. Locally, set it with dotnet user-secrets.");

builder.Services.AddNpgsqlDataSource(connectionString);
builder.Services.AddOpenApi();

builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database", tags: ["ready"], timeout: TimeSpan.FromSeconds(3));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Liveness: is the process up? Runs no checks, so a database outage never triggers restarts.
app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });

// Readiness: can we serve traffic? Runs the checks tagged "ready" (the database).
app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = check => check.Tags.Contains("ready") });

app.Run();

// Lets the test project start the API in-memory with WebApplicationFactory<Program>.
public partial class Program;
