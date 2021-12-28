namespace Competitions.Domain.Mapping
{
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection RegisterMapper(this IServiceCollection serviceCollection, params Assembly[] assemblies)
        {
            return serviceCollection.AddAutoMapper(assemblies);
        }
    }
}