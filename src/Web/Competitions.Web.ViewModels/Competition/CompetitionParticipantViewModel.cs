namespace Competitions.Web.ViewModels.Competition
{
    using System.Collections.Generic;
    using Customer;
    using Data.Models.Competition;
    using Domain.Mapping.Mapping;
    using Rating;
    using Team;

    public class CompetitionParticipantViewModel : IMapFrom<CompetitionParticipant>
    {
        public CompetitionViewModel Competition { get; set; }
        
        public ParticipantViewModel Participant { get; set; }
        
        public double? Points { get; set; }
        
        public TeamViewModel Team { get; set; }
        
        public ICollection<MatchViewModel> Matches { get; set; }
        
        public ICollection<ParticipantRatingViewModel> Ratings { get; set; }
    }
}