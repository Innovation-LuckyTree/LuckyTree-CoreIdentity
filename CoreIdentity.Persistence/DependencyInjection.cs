using CoreIdentity.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoreIdentity.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<CoreIdentityDbContext>(opts => opts.UseNpgsql(connectionString));
        services.AddScoped<ICoreIdentityDbContext>(provider => provider.GetService<CoreIdentityDbContext>());

        return services;
    }
}
