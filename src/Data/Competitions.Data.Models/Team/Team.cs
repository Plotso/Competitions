namespace Competitions.Data.Models.Team
{
    using System.Collections.Generic;
    using Common.Models;
    using Competition;

    public class Team : BaseDeletableModel<int>
    {
        public string Name { get; set; }
        
        public virtual ICollection<TeamParticipant> Participants { get; set; }
        
        public virtual ICollection<CompetitionParticipant> Competitions { get; set; }
    }
}