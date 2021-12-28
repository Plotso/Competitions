namespace Competitions.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Models;

    public class SportsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Sports.Any())
            {
                return;
            }

            var sports = new List<Sport>
            {
                new() { Name = "Football", IsVerified = true },
                new() { Name = "Tennis", IsVerified = true },
                new() { Name = "Chess", IsVerified = true },
                new() { Name = "ESport", IsVerified = true },
                new() { Name = "Squash", IsVerified = true },
                new() { Name = "Darts", IsVerified = true }
            };

            foreach (var sport in sports)
            {
                await dbContext.AddAsync(sport);
            }
        }
    }
}