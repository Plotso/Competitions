namespace Competitions.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Castle.Core.Internal;
    using Common;
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
    using ViewModels.Team;

    public class CompetitionsController : Controller
    {
        private readonly ICompetitionsService _competitionsService;
        private readonly ISportsService _sportsService;
        private readonly ITeamsService _teamsService;
        private readonly ICustomersService _customersService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CompetitionsController> _logger;

        public CompetitionsController(
            ICompetitionsService competitionsService,
            ISportsService sportsService,
            ITeamsService teamsService,
            ICustomersService customersService,
            UserManager<ApplicationUser> userManager,
            ILogger<CompetitionsController> logger)
        {
            _competitionsService = competitionsService;
            _sportsService = sportsService;
            _teamsService = teamsService;
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
        
        public async Task<IActionResult> ById(int id)
        {
            var competition = _competitionsService.GetById<CompetitionViewModel>(id);
            if (competition == null)
                return NotFound();

            return View(competition);
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
            
            return competitions.Any() ?
                View(competitions.ToList()) :
                View("NoCompetitions");
        }

        [Authorize]
        public async Task<IActionResult> InitiateSignIn(int competitionId, bool isTeamCompetition)
        {
            try
            {
                var participantId = await GetUserParticipantId();
                var isAlreadySignedIn = _competitionsService.IsParticipantAlreadySignedIn(competitionId, participantId);
                if (!isAlreadySignedIn)
                {
                    if (isTeamCompetition)
                    {
                        var participantTeams = _teamsService.GetAllByParticipantId<TeamViewModel>(participantId).ToList();
                        if (!participantTeams.Any())
                        {
                            return RedirectToAction("Create", "Teams");
                        }

                        if (participantTeams.Count > 1)
                        {
                            var selectTeamViewModel = new SelectTeamViewModel
                            {
                                CompetitionId = competitionId,
                                Teams = participantTeams.Select(t => new SelectListItem($"{t.Name}", t.Id.ToString())).ToList()
                            };

                            return View("SelectTeam", selectTeamViewModel);
                        }

                        await _competitionsService.SignParticipant(competitionId, participantId, participantTeams.First().Id);
                    }
                    else
                    {
                        await _competitionsService.SignParticipant(competitionId, participantId);
                    }
                }

                return RedirectToAction(nameof(ById), new {id = competitionId});
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SelectTeam(SelectTeamViewModel model)
        {
            if (!ModelState.IsValid || model.SelectedTeamId.IsNullOrEmpty())
            {
                return View(model);
            }

            try
            {
                if (!int.TryParse(model.SelectedTeamId, out var teamId))
                {
                    throw new InvalidOperationException(
                        $"Provided team id {model.SelectedTeamId} could not be cast to an integer");
                }
                var participantId = await GetUserParticipantId();
                await _competitionsService.SignParticipant(model.CompetitionId, participantId, teamId);
                return RedirectToAction(nameof(ById), new {id = model.CompetitionId});
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return RedirectToAction("Error", "Home");
            }
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

        [Authorize]
        public async Task<IActionResult> Modify(int id)
        {
            var competition = _competitionsService.GetById<CompetitionInputModel>(id);
            if (competition == null)
                return NotFound();
            
            var isAuthorised = await IsOrganiserOrAdmin(competition.OrganiserId);
            if (!isAuthorised)
                return RedirectToAction("Forbidden", "Home");
            
            var sports = _sportsService.GetAll<SportModifyInputModel>();
            var viewModel = new CompetitionModifyInputModel
            {
                Id = id,
                Sports = sports.Select(s => new SelectListItem($"{s.Name}", s.Id.ToString())).ToList(),
                Competition = competition
            };
            return View(viewModel);

        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Modify(CompetitionModifyInputModel inputModel)
        {
            if (!ModelState.IsValid || inputModel.Competition.SportId.IsNullOrEmpty() || inputModel.Competition.TypeId.IsNullOrEmpty())
            {
                return View(inputModel);
            }

            try
            {
                await _competitionsService.EditAsync(inputModel);
                return RedirectToAction(nameof(ById), new { id = inputModel.Id});
            }
            catch (MissingSportException e)
            {
                var inputJson = JsonConvert.SerializeObject(inputModel, Formatting.Indented);
                _logger.LogError("Couldn't update competition with id {competitionId} due to missing sport. Request: {newLine} {inputJson}", inputModel.Id, Environment.NewLine, inputJson);
                return RedirectToAction("Error", "Home");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var competition = _competitionsService.GetById<CompetitionInputModel>(id);
            if (competition == null)
                return NotFound();
            
            var isAuthorised = await IsOrganiserOrAdmin(competition.OrganiserId);
            if (!isAuthorised)
                return RedirectToAction("Forbidden", "Home");
            
            var viewModel = new CompetitionModifyInputModel
            {
                Id = id,
                Competition = competition
            };
            return View(viewModel);

        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(CompetitionModifyInputModel inputModel, string onSubmitAction)
        {
            if (onSubmitAction.IsNullOrEmpty() || onSubmitAction == "Откажи")
            {
                return RedirectToAction(nameof(All));
            }
            
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            try
            {
                await _competitionsService.DeleteAsync(inputModel.Id);
                //ToDo: Delete also related reservations, competitionParticipants & so on
                return RedirectToAction(nameof(All));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return RedirectToAction("Error", "Home");
            }
        }

        private async Task<string> GetUserParticipantId()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            return _customersService.GetParticipantId(currentUser.Id);
        }

        private async Task<bool> IsOrganiserOrAdmin(string competitionOrganiserId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var organiserId = _customersService.GetOrganiserId(currentUser.Id);
            return organiserId == competitionOrganiserId || await IsAdmin();
        }

        private async Task<bool> IsAdmin()
        {
            var user = await _userManager.GetUserAsync(User);
            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles.Any(r => r == GlobalConstants.AdministratorRoleName);
        }
    }
}