using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace PantryChef.Api.Health;

/// <summary>Answers "can the API reach PostgreSQL right now?" by running SELECT 1.</summary>
public sealed class DatabaseHealthCheck(NpgsqlDataSource dataSource) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var command = dataSource.CreateCommand("SELECT 1");
            await command.ExecuteScalarAsync(cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            // The exception is logged by the health check system, never sent in the HTTP response.
            return HealthCheckResult.Unhealthy("Database unreachable", ex);
        }
    }
}
