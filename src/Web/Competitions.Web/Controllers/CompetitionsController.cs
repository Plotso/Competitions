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

    public class CompetitionsController : Controller
    {
        private readonly ICompetitionsService _competitionsService;
        private readonly ILogger<CompetitionsController> _logger;

        public CompetitionsController(ICompetitionsService competitionsService, ILogger<CompetitionsController> logger)
        {
            _competitionsService = competitionsService;
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
            var statuses = isFinished
                ? new[] { CompetitionStatus.Finished }
                : new[] { CompetitionStatus.Active, CompetitionStatus.Upcoming };
            var competitions =
                _competitionsService.GetAllBySportAndStatuses<CompetitionViewModel>(sportId, statuses).ToList();

            return View(competitions.ToList());
        }
    }
}