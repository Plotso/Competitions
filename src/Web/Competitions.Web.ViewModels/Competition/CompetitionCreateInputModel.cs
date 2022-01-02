namespace Competitions.Web.ViewModels.Competition
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Data.Models.Competition;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CompetitionCreateInputModel
    {
        public List<SelectListItem> Sports { get; set; }

        public List<SelectListItem> TypesBG = new List<SelectListItem>
        {
            new SelectListItem("Турнир", 1.ToString()),
            new SelectListItem("Лига", 2.ToString()),
            new SelectListItem("Друг", 0.ToString())
        };
        
        public CompetitionInputModel Competition { get; set; }
    }
}