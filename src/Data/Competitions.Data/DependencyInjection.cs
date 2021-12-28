namespace Competitions.Data
{
    using Common.Repositories;
    using Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;
    using Models;
    using Repositories;

    public static class DependencyInjection
    {
        public static IServiceCollection RegisterDataAccessLayer(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            
            serviceCollection.AddDbContext<ApplicationDbContext>(
                options => options
                    .UseLazyLoadingProxies()
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );
            
            serviceCollection
                .AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            return serviceCollection.RegisterRepositories();
        }
        
        private static IServiceCollection RegisterRepositories(this IServiceCollection serviceCollection) 
            => serviceCollection
                .AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>))
                .AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
    }
}