
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class PetVilleFactory : WebApplicationFactory<Program>
{
    static PetVilleFactory()
    {
        Environment.SetEnvironmentVariable(
            "Jwt__Key",
            Convert.ToBase64String(new byte[32])
        );
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
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

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var conteudo = await response.Content.ReadAsStringAsync();

        Assert.Contains("PetVille API iniciada", conteudo);
    }
}
