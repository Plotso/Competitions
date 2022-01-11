namespace Competitions.Web.ViewModels.Team
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Data.Models.Team;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class SelectTeamViewModel
    {
        public int CompetitionId { get; set; }
        
        [Required]
        public string SelectedTeamName { get; set; }
        
        public ICollection<TeamViewModel> ActualTeams { get; set; }
        
        public List<SelectListItem> Teams { get; init; }
    }
}