namespace Competitions.Data.Models.Rating
{
    using System.ComponentModel.DataAnnotations;
    using Common.Models;
    using Competition;
    using Customer;

    public class ParticipantRating : BaseDeletableModel<int>
    {
        [Range(0.0, 5.0)]
        public double Score { get; set; }
        
        [Required]
        public string CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int CompetitionParticipantId { get; set; }
        public virtual CompetitionParticipant CompetitionParticipant { get; set; }
    }
}