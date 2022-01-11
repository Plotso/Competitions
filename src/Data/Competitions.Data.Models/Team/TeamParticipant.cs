namespace Competitions.Data.Models.Team
{
    using System.ComponentModel.DataAnnotations;
    using Common.Models;
    using Customer;

    public class TeamParticipant : BaseDeletableModel<int>
    {
        [Required]
        public string ParticipantId { get; set; }
        
        public virtual Participant Participant { get; set; }
        
        public int TeamId { get; set; }
        
        public virtual Team Team { get; set; }
    }
}