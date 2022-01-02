namespace Competitions.Web.ViewModels.Competition
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CompetitionModifyInputModel
    {
        public int Id { get; set; }
        
        public List<SelectListItem> Sports { get; set; }
        
        public CompetitionInputModel Competition { get; set; }

        public List<SelectListItem> TypesBG = new List<SelectListItem>
        {
            new SelectListItem("Турнир", 1.ToString()),
            new SelectListItem("Лига", 2.ToString()),
            new SelectListItem("Друг", 0.ToString())
        };
    }
}