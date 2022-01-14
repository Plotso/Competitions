namespace Competitions.Web.Healthchecks
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.BL.Services.Interfaces;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using ViewModels.Sport;

    public class SqlHealthcheck : IHealthCheck
    {
        private readonly ISportsService _sportsService;

        public SqlHealthcheck(ISportsService sportsService)
        {
            _sportsService = sportsService;
        }
        
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var sports = _sportsService.GetAll<SportViewModel>();

            var result = sports.Any()
                ? HealthCheckResult.Healthy()
                : HealthCheckResult.Unhealthy("Failed to obtain data from the database");
            return Task.FromResult(result);
        }
    }
}