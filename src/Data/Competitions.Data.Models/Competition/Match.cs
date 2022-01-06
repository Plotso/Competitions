namespace Competitions.Data.Models.Competition
{
    using System;
    using System.Collections.Generic;
    using Common.Models;

    public class Match : BaseDeletableModel<int>
    {
        public DateTime Date { get; set; }

        public int CompetitionParticipantA { get; set; }
        
        public int CompetitionParticipantB { get; set; } 
        
        public MatchStatus Status { get; set; }
        
        public bool IsCloseLoss { get; set; }
        
        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }
        
        public virtual ICollection<Score> Scores { get; set; }
    }

    public enum MatchStatus
    {
        NotStarted,
        InProgress,
        Finished
    }
}