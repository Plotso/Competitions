namespace Competitions.Data
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Models.Interfaces;
    using Configuration;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Models.Administration;
    using Models.Competition;
    using Models.Customer;
    using Models.Partner;
    using Models.Rating;
    using Models.Team;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
            typeof(ApplicationDbContext).GetMethod(
                nameof(SetIsDeletedQueryFilter),
                BindingFlags.NonPublic | BindingFlags.Static);

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Organiser> Organisers { get; set; }
        
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamParticipant> TeamParticipants { get; set; }

        public DbSet<Sport> Sports { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<CompetitionParticipant> CompetitionParticipants { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Score> Scores { get; set; }
        
        public DbSet<ParticipantRating> ParticipantRatings { get; set; }
        public DbSet<OrganiserRating> OrganiserRatings { get; set; }
        public DbSet<CompetitionRating> CompetitionRatings { get; set; }
        public DbSet<PlatformReview> PlatformReviews { get; set; }
        
        public DbSet<Partner> Partners { get; set; }
        
        public DbSet<Ground> Grounds { get; set; }
        
        public DbSet<Referee> Referees { get; set; }
        
        public DbSet<Reservation> Reservations { get; set; }
        
        public DbSet<AdministrationConfiguration> AdministrationConfigurations { get; set; }

        public override int SaveChanges() => SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Needed for Identity models configuration
            base.OnModelCreating(builder);

            ConfigureUserIdentityRelations(builder);

            EntityIndexesConfiguration.Configure(builder);

            var entityTypes = builder.Model.GetEntityTypes().ToList();

            // Set global query filter for not deleted entities only
            var deletableEntityTypes = entityTypes
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
                method.Invoke(null, new object[] { builder });
            }

            // Disable cascade delete
            var foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
            where T : class, IDeletableEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        // Applies configurations
        private void ConfigureUserIdentityRelations(ModelBuilder builder)
             => builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        private void ApplyAuditInfoRules()
        {
            var changedEntries = ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}