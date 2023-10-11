using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Api.IntegrationTests.Common;

public abstract class ApiTests
    : IClassFixture<ApiWebApplicationFactory>,
      IDisposable
{
    private readonly IServiceScope _scope;
    protected readonly HttpClient ApiClient;
    protected readonly ApiDbContext DbContext;

    protected ApiTests(ApiWebApplicationFactory factory)
    {
        _scope = factory.Services.CreateScope();
        ApiClient = factory.CreateClient();
        DbContext = _scope.ServiceProvider.GetRequiredService<ApiDbContext>();
        DbContext.Database.Migrate();
    }

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        _scope.Dispose();
        DbContext.Dispose();
    }
}