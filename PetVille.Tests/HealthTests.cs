
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

public class PetVilleFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = Convert.ToBase64String(new byte[32])
            });
        });
    }
}

public class HealthTests : IClassFixture<PetVilleFactory>
{
    private readonly HttpClient _client;

    public HealthTests(PetVilleFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Health_DeveRetornarSucesso()
    {
        var response = await _client.GetAsync("/api/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var conteudo = await response.Content.ReadAsStringAsync();

        Assert.Contains("PetVille API iniciada", conteudo);
    }
}
