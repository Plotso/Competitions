namespace Competitions.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Diagnostics;
    using System.Linq;
    using Domain.BL.Services.Interfaces;
    using ViewModels;
    using ViewModels.Sport;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISportsService _sportsService;

        public HomeController(ILogger<HomeController> logger, ISportsService sportsService)
        {
            _logger = logger;
            _sportsService = sportsService;
        }

        public IActionResult Index()
        {
            return View();
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
    }
}
