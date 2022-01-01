namespace Competitions.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    public class CompetitionsController : Controller
    {
        public async Task<IActionResult> FullCompetitionsList()
        {
            throw new NotImplementedException();
        }
        
        public async Task<IActionResult> Upcoming()
        {
            throw new NotImplementedException();
        }
        
        public async Task<IActionResult> Active()
        {
            throw new NotImplementedException();
        }
        
        public async Task<IActionResult> Finished()
        {
            throw new NotImplementedException();
        }
        
        public async Task<IActionResult> BySport(int sportId, bool isFinished)
        {
            throw new NotImplementedException();
        }
    }
}