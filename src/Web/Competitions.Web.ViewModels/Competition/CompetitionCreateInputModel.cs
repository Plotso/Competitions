namespace Competitions.Web.ViewModels.Competition
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Data.Models.Competition;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CompetitionCreateInputModel
    {
        public List<SelectListItem> Sports { get; set; }
        
        public CompetitionInputModel Competition { get; set; }
    }
}