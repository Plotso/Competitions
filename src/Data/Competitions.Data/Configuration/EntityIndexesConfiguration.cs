namespace Competitions.Data.Configuration
{
    using System.Linq;
    using Common.Models.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Models.Customer;
    using Models.Team;

    internal static class EntityIndexesConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            // UniqueID index
            var personEntities = modelBuilder.Model.GetEntityTypes().Where(e => e.ClrType != null && typeof(Customer).IsAssignableFrom(e.ClrType));
            foreach (var entity in personEntities)
            {
                modelBuilder
                    .Entity(entity.ClrType)
                    .HasIndex(nameof(Customer.ApplicationUserId))
                    .IsUnique();
            }
            var teamEntities = modelBuilder.Model.GetEntityTypes().Where(e => e.ClrType != null && typeof(Team).IsAssignableFrom(e.ClrType));
            foreach (var entity in teamEntities)
            {
                modelBuilder
                    .Entity(entity.ClrType)
                    .HasIndex(nameof(Team.Name))
                    .IsUnique();
            }
            
            // IDeletableEntity.IsDeleted index
            var deletableEntityTypes = modelBuilder.Model
                .GetEntityTypes()
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                modelBuilder.Entity(deletableEntityType.ClrType).HasIndex(nameof(IDeletableEntity.IsDeleted));
            }
        }
    }
}