namespace Competitions.Data.Models.Competition
{
    using Common.Models;

    public class Score : BaseDeletableModel<string>
    {
        public int MatchId { get; set; }
        public virtual Match Match { get; set; }
        
        public double CompetitionParticipantAScore { get; set; }
        
        public double CompetitionParticipantBScore { get; set; }
        
        public ScorePeriod Period { get; set; } 
        
        public ScoreOutcome Result { get; set; } 
        
        public string Details { get; set; }
    }

    public enum ScorePeriod
    {
        Unknown,
        FirstHalf,
        SecondHalf,
        FirstQuarter,
        SecondQuarter,
        ThirdQuarter,
        FourthQuarter,
        Overtime,
        Final
    }
    public enum ScoreOutcome
    {
        SideA,
        SideB,
        Draw
    }
}