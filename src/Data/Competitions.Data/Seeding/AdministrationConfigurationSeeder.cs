namespace Competitions.Data.Seeding
{
    using System;
    using System.Threading.Tasks;
    using Models.Administration;

    public class AdministrationConfigurationSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var configuration = new AdministrationConfiguration {IsFrozen = false};

            await dbContext.AddAsync(configuration);
        }
    }
}