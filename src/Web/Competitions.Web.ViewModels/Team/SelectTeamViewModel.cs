namespace Competitions.Web.ViewModels.Team
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class SelectTeamViewModel
    {
        public int CompetitionId { get; set; }
        
        [Required]
        public string SelectedTeamId { get; set; }
        
        public List<SelectListItem> Teams { get; init; }
    }
}