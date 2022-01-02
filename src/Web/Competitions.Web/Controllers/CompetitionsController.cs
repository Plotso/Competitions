namespace Competitions.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Castle.Core.Internal;
    using Data.Models;
    using Data.Models.Competition;
    using Domain.BL.Enums;
    using Domain.BL.Exceptions;
    using Domain.BL.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using ViewModels.Competition;
    using ViewModels.Sport;

    public class CompetitionsController : Controller
    {
        private readonly ICompetitionsService _competitionsService;
        private readonly ISportsService _sportsService;
        private readonly ICustomersService _customersService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CompetitionsController> _logger;

        public CompetitionsController(
            ICompetitionsService competitionsService,
            ISportsService sportsService,
            ICustomersService customersService,
            UserManager<ApplicationUser> userManager,
            ILogger<CompetitionsController> logger)
        {
            _competitionsService = competitionsService;
            _sportsService = sportsService;
            _customersService = customersService;
            _userManager = userManager;
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

        [Authorize]
        public async Task<IActionResult> Create()
        {
            var sports = _sportsService.GetAll<SportModifyInputModel>();
            var viewModel = new CompetitionCreateInputModel
            {
                Sports = sports.Select(s => new SelectListItem($"{s.Name}", s.Id.ToString())).ToList()
            };
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompetitionCreateInputModel inputModel)
        {
            if (!ModelState.IsValid || inputModel.Competition.SportId.IsNullOrEmpty() || inputModel.Competition.TypeId.IsNullOrEmpty())
            {
                return View(inputModel);
            }

            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var organiserId = _customersService.GetOrganiserId(currentUser.Id);
                await _competitionsService.CreateAsync(inputModel, organiserId);
                return RedirectToAction(nameof(All));
            }
            catch (MissingSportException e)
            {
                var inputJson = JsonConvert.SerializeObject(inputModel, Formatting.Indented);
                _logger.LogError("Couldn't create competition due to missing sport. Request: {newLine} {inputJson}", Environment.NewLine, inputJson);
                return RedirectToAction("Error", "Home");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}