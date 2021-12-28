namespace Competitions.Data.Models.Rating
{
    using System.ComponentModel.DataAnnotations;
    using Common.Models;
    using Customer;

    public class OrganiserRating : BaseDeletableModel<int>
    {
        [Range(0.0, 5.0)]
        public double Score { get; set; }
        
        [Required]
        public string CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        
        [Required]
        public string OrganiserId { get; set; }
        public virtual Organiser Organiser { get; set; }
    }
}