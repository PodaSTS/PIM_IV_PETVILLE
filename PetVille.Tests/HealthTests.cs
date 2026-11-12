
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

        // Configuração de conexão fictícia apenas para os testes.
        // Não utiliza um banco MySQL real.
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DefaultConnection"] =
                        "Server=localhost;Database=petville_test;User=root;Password=test;"
                }
            );
        });

        // Substitui o banco MySQL pelo banco em memória.
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

    private async Task<string> CadastrarCliente()
    {
        var dados = new
        {
            Nome = "Cliente Teste",
            Email = $"teste{Guid.NewGuid():N}@email.com",
            Telefone = "11999999999",
            Senha = "PetVille@123"
        };

        var response = await _client.PostAsJsonAsync(
            "/api/clientes/cadastro",
            dados
        );

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        return dados.Email;
    }

    [Fact]
    public async Task Health_DeveRetornarSucesso()
    {
        var response = await _client.GetAsync("/api/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var conteudo = await response.Content.ReadAsStringAsync();

        Assert.Contains("PetVille API iniciada", conteudo);
    }

    [Fact]
    public async Task MeuPerfil_SemToken_DeveRetornarUnauthorized()
    {
        var response = await _client.GetAsync("/api/clientes/me");

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode
        );
    }

    [Fact]
    public async Task Cadastro_DeveRetornarSucesso()
    {
        await CadastrarCliente();
    }

    [Fact]
    public async Task Login_SenhaIncorreta_DeveRetornarUnauthorized()
    {
        var email = await CadastrarCliente();

        var response = await _client.PostAsJsonAsync(
            "/api/clientes/login",
            new
            {
                Email = email,
                Senha = "SenhaErrada123"
            }
        );

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode
        );
    }

    [Fact]
    public async Task Login_ComToken_DevePermitirAcessoAoPerfil()
    {
        var email = await CadastrarCliente();

        var login = await _client.PostAsJsonAsync(
            "/api/clientes/login",
            new
            {
                Email = email,
                Senha = "PetVille@123"
            }
        );

        Assert.Equal(HttpStatusCode.OK, login.StatusCode);

        var conteudo = await login.Content.ReadAsStringAsync();

        using var json = JsonDocument.Parse(conteudo);

        var token = json.RootElement
            .GetProperty("token")
            .GetString();

        Assert.False(string.IsNullOrWhiteSpace(token));

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var perfil = await _client.GetAsync("/api/clientes/me");

        Assert.Equal(HttpStatusCode.OK, perfil.StatusCode);

        _client.DefaultRequestHeaders.Authorization = null;
    }
}
