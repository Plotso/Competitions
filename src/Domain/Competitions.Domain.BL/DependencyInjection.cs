namespace Competitions.Domain.BL
{
    using System.Reflection;
    using Domain.Mapping;
    using Enums;
    using Microsoft.Extensions.DependencyInjection;
    using Services;
    using Services.Interfaces;
    using Web.ViewModels;
    using Web.ViewModels.Sport;

    public static class DependencyInjection
    {
        public static IServiceCollection RegisterDomainServices(this IServiceCollection serviceCollection)
        {
            return 
                serviceCollection.RegisterMapper(typeof(SportViewModel).GetTypeInfo().Assembly)
                    .AddTransient<ISportsService, SportsService>()
                    .AddTransient<ICompetitionsService, CompetitionsService>();
        }
    }
}