namespace Competitions.Web.ViewModels.Team
{
    using System.Collections.Generic;
    using Competition;
    using Data.Models.Team;
    using Domain.Mapping.Mapping;

    public class TeamViewModel : IMapFrom<Team>
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string CreatorCustomerId { get; set; }
        
        public ICollection<TeamParticipantViewModel> Participants { get; set; }
    }
}