namespace Competitions.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using BusinessViewModels;
    using Castle.Core.Internal;
    using Data.Models;
    using Domain.BL.Models;
    using Domain.BL.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using ViewModels;
    using ViewModels.Customer;
    using ViewModels.Rating;
    using ViewModels.Sport;
    using ViewModels.Team;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISportsService _sportsService;
        private readonly ICustomersService _customersService;
        private readonly IPlatformReviewsService _platformReviewsService;
        private readonly ICompetitionsService _competitionsService;
        private readonly ITeamsService _teamsService;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(
            ILogger<HomeController> logger, 
            ISportsService sportsService, 
            ICustomersService customersService,
            IPlatformReviewsService platformReviewsService,
            ICompetitionsService competitionsService,
            ITeamsService teamsService,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _sportsService = sportsService;
            _customersService = customersService;
            _platformReviewsService = platformReviewsService;
            _competitionsService = competitionsService;
            _teamsService = teamsService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            const int rankingResultMembers = 3;
            var statusRanking = _competitionsService.GetTopNByCriteria(rankingResultMembers, RankingCriteria.Status).Select(TranslateStatusToBulgarian);
            var sportsRanking = _competitionsService.GetTopNByCriteria(rankingResultMembers, RankingCriteria.Sport);
            var locationRanking = _competitionsService.GetTopNByCriteria(rankingResultMembers, RankingCriteria.Location);
            var viewModel = new IndexViewModel
            {
                RankingByStatus = statusRanking,
                RankingBySport = sportsRanking,
                RankingByLocation = locationRanking
            };
            return View(viewModel);
        }

        public IActionResult Sports()
        {
            var sports = _sportsService.GetAll<SportViewModel>().ToList();
            return View(sports);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Terms()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View(new PlatformReviewViewModel());
        }

        [Authorize]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Contact(PlatformReviewViewModel model)
        {
            if (!ModelState.IsValid || (model.Score == 0.0 && model.Comment.IsNullOrEmpty()))
            {
                return View(new PlatformReviewViewModel());
            }

            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var customerId = _customersService.GetCustomerId(currentUser.Id);
                await _platformReviewsService.CreateAsync(model, customerId);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Forbidden()
        {
            return View();
        }

        public IActionResult PageNotFound()
        {
            return View();
        }

        /*[Route("/sitemap.xml")]
        public IActionResult Sitemap()
        {
        }*/

        [Authorize] // ToDo: Commented out due to being dangerous for the SQL
        private async Task<IActionResult> UserDetails(string participantId = null)
        {
            var id = participantId;
            if (id == null)
            {
                id = await GetUserParticipantId();
            }

            var teams = _teamsService.GetAllByParticipantId<TeamViewModel>(id).ToList();
            var participant = _customersService.GetParticipant<ParticipantViewModel>(id);
            var teamCompetitions = teams.SelectMany(t => t.Competitions.Select(c => c.Competition));
            var participantCompetitions = participant.Competitions.Select(c => c.Competition).ToList();
            var organiser = _customersService.GetOrganiserByParticipantId<OrganiserViewModel>(id);
            var viewModel = new CustomerInfoViewModel
            {
                IndividualCompetitions = participantCompetitions,
                TeamCompetitions = teamCompetitions,
                OrganisedCompetitions = organiser.Competitions,
                Teams = teams
            };

            return View(viewModel);
        }

        private TopRankingDTO TranslateStatusToBulgarian(TopRankingDTO input)
        {
            var bulgarianGroupName = input.GroupName == "Upcoming" ?
                "Предстоящии" : input.GroupName == "Finished" ?
                    "Приключили" : "Активни";
            input.GroupName = bulgarianGroupName;
            return input;
        }

        private async Task<string> GetUserParticipantId()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            return _customersService.GetParticipantId(currentUser.Id);
        }
    }
}
