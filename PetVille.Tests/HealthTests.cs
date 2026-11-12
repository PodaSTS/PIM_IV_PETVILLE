
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PetVille.API.Data;
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

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<PetVilleContext>>();
            services.RemoveAll<PetVilleContext>();

            services.AddDbContext<PetVilleContext>(options =>
                options.UseInMemoryDatabase("PetVilleTestDb")
            );
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

    // TESTE 1: Verifica se a API está funcionando
    [Fact]
    public async Task Health_DeveRetornarSucesso()
    {
        var response = await _client.GetAsync("/api/health");

        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            response.StatusCode
        );

        var conteudo = await response.Content.ReadAsStringAsync();

        Assert.Contains("PetVille API iniciada", conteudo);
    }

    // TESTE 2: Verifica se o perfil exige autenticação
    [Fact]
    public async Task MeuPerfil_SemToken_DeveRetornarUnauthorized()
    {
        var response = await _client.GetAsync("/api/clientes/me");

        Assert.Equal(
            System.Net.HttpStatusCode.Unauthorized,
            response.StatusCode
        );
    }
}
