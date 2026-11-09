using System.Net;

namespace PantryChef.Api.Tests;

public sealed class HealthTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    [Fact]
    public async Task Ready_returns_200_when_database_is_reachable()
    {
        var response = await factory.CreateClient().GetAsync("/health/ready");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Live_returns_200_when_database_is_reachable()
    {
        var response = await factory.CreateClient().GetAsync("/health/live");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}

public sealed class UnreachableDatabaseHealthTests(UnreachableDatabaseApiFactory factory)
    : IClassFixture<UnreachableDatabaseApiFactory>
{
    [Fact]
    public async Task Live_returns_200_even_when_database_is_down()
    {
        var response = await factory.CreateClient().GetAsync("/health/live");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Ready_returns_503_when_database_is_down()
    {
        var response = await factory.CreateClient().GetAsync("/health/ready");

        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
    }

    [Fact]
    public async Task Ready_does_not_leak_error_details()
    {
        var body = await (await factory.CreateClient().GetAsync("/health/ready")).Content.ReadAsStringAsync();

        Assert.Equal("Unhealthy", body);
    }
}
