namespace Competitions.Web.ViewModels.Team
{
    using Customer;
    using Data.Models.Team;
    using Domain.Mapping.Mapping;

    public class TeamParticipantViewModel : IMapFrom<TeamParticipant>
    {
        public ParticipantViewModel ParticipantViewModel { get; set; }
        
        public TeamViewModel Team { get; set; }
    }
}