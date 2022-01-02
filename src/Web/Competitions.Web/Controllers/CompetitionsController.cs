namespace Competitions.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.BL.Enums;
    using Domain.BL.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using ViewModels.Competition;
    using ViewModels.Sport;

    public class CompetitionsController : Controller
    {
        private readonly ICompetitionsService _competitionsService;
        private readonly ISportsService _sportsService;
        private readonly ILogger<CompetitionsController> _logger;

        public CompetitionsController(ICompetitionsService competitionsService, ISportsService sportsService, ILogger<CompetitionsController> logger)
        {
            _competitionsService = competitionsService;
            _sportsService = sportsService;
            _logger = logger;
        }
        public async Task<IActionResult> All()
        {
            var upcomingCompetitions =
                _competitionsService.GetAll<CompetitionViewModel>();

            return View(upcomingCompetitions.ToList());
        }
        
        public async Task<IActionResult> Upcoming()
        {
            var upcomingCompetitions =
                _competitionsService.GetAllByStatus<CompetitionViewModel>(CompetitionStatus.Upcoming);

            return View(upcomingCompetitions.ToList());
        }
        
        public async Task<IActionResult> Active()
        {
            var competitions =
                _competitionsService.GetAllByStatus<CompetitionViewModel>(CompetitionStatus.Active);

            return View(competitions.ToList());
        }
        
        public async Task<IActionResult> Finished()
        {
            var competitions =
                _competitionsService.GetAllByStatus<CompetitionViewModel>(CompetitionStatus.Finished);

            return View(competitions.ToList());
        }
        
        public async Task<IActionResult> BySport(int sportId, bool isFinished)
        {
            var sport = _sportsService.GetById<SportViewModel>(sportId);
            if (sport == null)
            {
                _logger.LogDebug("Sport with ID: {sportId} is missing from the DB", sportId);
                return RedirectToAction("PageNotFound", "Home");
            }
            
            var statuses = isFinished
                ? new[] { CompetitionStatus.Finished }
                : new[] { CompetitionStatus.Active, CompetitionStatus.Upcoming };
            var competitions =
                _competitionsService.GetAllBySportAndStatuses<CompetitionViewModel>(sportId, statuses).ToList();
            // ToDo: Return another view in case of no competitions
            return View(competitions.ToList());
        }
    }
}