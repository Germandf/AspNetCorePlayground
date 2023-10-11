using Api.Models;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Api.IntegrationTests.Common;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private IContainer _postgresContainer = new ContainerBuilder()
        .WithImage("postgres:12.16")
        .WithPortBinding("5432", true)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
        .WithEnvironment("POSTGRES_USER", "postgres")
        .WithEnvironment("POSTGRES_PASSWORD", "postgres")
        .WithEnvironment("POSTGRES_INITDB_ARGS", "--lc-collate='en_US.UTF-8' --lc-ctype='en_US.UTF-8'")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ApiDbContext>));
            var postgresConnString = $"Host={_postgresContainer.Hostname}:{_postgresContainer.GetMappedPublicPort(5432)};Username=postgres;Password=postgres;Database=AspNetCorePlayground";
            services.AddDbContext<ApiDbContext>(options => options.UseNpgsql(postgresConnString));
        });
    }

    async Task IAsyncLifetime.InitializeAsync()
    {
        await _postgresContainer.StartAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _postgresContainer.StopAsync();
    }
}
