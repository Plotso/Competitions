namespace Competitions.Data.Models.Competition
{
    using Common.Models;

    public class Score : BaseDeletableModel<string>
    {
        public int CompetitionParticipantAId { get; set; }
        public int CompetitionParticipantBId { get; set; }
        
        public double CompetitionParticipantAScore { get; set; }
        public double CompetitionParticipantBScore { get; set; }
        public ScoreOutcome Result { get; set; } 
    }
    public enum ScoreOutcome
    {
        InProgress,
        SideA,
        SideB,
        Draw
    }
}