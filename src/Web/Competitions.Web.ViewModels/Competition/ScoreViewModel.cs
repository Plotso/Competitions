namespace Competitions.Web.ViewModels.Competition
{
    using Data.Models.Competition;
    using Domain.Mapping.Mapping;

    public class ScoreViewModel : IMapFrom<Score>
    {
        public double CompetitionParticipantAScore { get; set; }
        
        public double CompetitionParticipantBScore { get; set; }
        
        public ScorePeriod Period { get; set; } 
        
        public ScoreOutcome Result { get; set; } 
        
        public string Details { get; set; }
    }
}