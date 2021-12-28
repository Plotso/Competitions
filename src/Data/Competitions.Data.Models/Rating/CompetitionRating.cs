namespace Competitions.Data.Models.Rating
{
    using System.ComponentModel.DataAnnotations;
    using Common.Models;
    using Competition;
    using Customer;

    public class CompetitionRating : BaseDeletableModel<int>
    {
        [Range(0.0, 5.0)]
        public double Score { get; set; }
        
        [Required]
        public string CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        
        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }
    }
}