namespace Competitions.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Castle.Core.Internal;
    using Data.Models;
    using Domain.BL.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using ViewModels.Team;

    public class TeamsController : Controller
    {
        private readonly ITeamsService _teamsService;
        private readonly ICustomersService _customersService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<TeamsController> _logger;

        public TeamsController(ITeamsService teamsService, ICustomersService customersService, UserManager<ApplicationUser> userManager, ILogger<TeamsController> logger)
        {
            _teamsService = teamsService;
            _customersService = customersService;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult ById(int id)
        {
            var team = _teamsService.GetById<TeamViewModel>(id);
            return View(team);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View(new TeamInputModel());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var customerId = _customersService.GetCustomerId(currentUser.Id);
                var teamId = await _teamsService.CreateAsync(inputModel, customerId);
                return RedirectToAction(nameof(ById), new {id = teamId});
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize]
        public IActionResult Modify(int id)
        {
            var team = _teamsService.GetById<TeamInputModel>(id);
            if (team == null)
                return NotFound();

            var model = new TeamModifyInputModel
            {
                Id = id,
                Team = team
            };
            
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Modify(TeamModifyInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            try
            {
                await _teamsService.EditAsync(inputModel);
                return RedirectToAction(nameof(ById), inputModel.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var team = _teamsService.GetById<TeamInputModel>(id);
            if (team == null)
                return NotFound();

            var model = new TeamModifyInputModel
            {
                Id = id,
                Team = team
            };
            
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(TeamModifyInputModel inputModel, string onSubmitAction)
        {
            if (onSubmitAction.IsNullOrEmpty() || onSubmitAction == "Откажи")
            {
                return RedirectToAction(nameof(ById), inputModel.Id);
            }
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            try
            {
                await _teamsService.DeleteAsync(inputModel.Id);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}