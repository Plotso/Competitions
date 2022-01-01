namespace Competitions.Web.ViewModels.Competition
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CompetitionModifyInputModel
    {
        public int Id { get; set; }
        
        public List<SelectListItem> Sports { get; set; }
        
        public CompetitionInputModel Competition { get; set; }
    }
}