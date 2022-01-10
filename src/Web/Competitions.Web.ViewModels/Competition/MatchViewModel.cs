namespace Competitions.Web.ViewModels.Competition
{
    using System;
    using System.Collections.Generic;
    using Data.Models.Competition;
    using Domain.Mapping.Mapping;

    public class MatchViewModel : IMapFrom<Match>
    {
        public DateTime Date { get; set; }

        public CompetitionParticipantViewModel SideA { get; set; }
        
        public CompetitionParticipantViewModel SideB { get; set; } 
        
        public MatchStatus Status { get; set; }
        
        public bool IsCloseLoss { get; set; }
        
        public CompetitionViewModel Competition { get; set; }
        
        public ICollection<ScoreViewModel> Scores { get; set; }
    }
}