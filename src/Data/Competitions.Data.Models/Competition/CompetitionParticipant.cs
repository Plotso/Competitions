namespace Competitions.Data.Models.Competition
{
    using System.Collections.Generic;
    using Common.Models;
    using Customer;
    using Rating;
    using Team;

    public class CompetitionParticipant : BaseDeletableModel<int>
    {
        public int CompetitionId { get; set; }
        
        public virtual Competition Competition { get; set; }
        
        public string ParticipantId { get; set; }
        
        public virtual Participant Participant { get; set; }
        
        public int? TeamId { get; set; }
        
        public virtual Team Team { get; set; }

        public virtual ICollection<Score> Scores { get; set; }
        
        public virtual ICollection<ParticipantRating> Ratings { get; set; }
    }
}