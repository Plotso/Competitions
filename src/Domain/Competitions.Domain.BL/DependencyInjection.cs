namespace Competitions.Domain.BL
{
    using System.Reflection;
    using Domain.Mapping;
    using Microsoft.Extensions.DependencyInjection;
    using Web.ViewModels;

    public static class DependencyInjection
    {
        public static IServiceCollection RegisterDomainServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection.RegisterMapper(typeof(SportViewModel).GetTypeInfo().Assembly);
        }
    }
}