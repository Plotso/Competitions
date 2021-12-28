namespace Competitions.Data.Models.Partner
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Common.Models;
    using Common.Models.Interfaces;
    using Competition;
    using Customer;

    public class Reservation : BaseDeletableModel<string>, ITestableEntity
    {
        [Required]
        public string CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        
        [Required]
        public int GroundId { get; set; }
        public virtual Ground Ground { get; set; }
        
        public int? CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }
        
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        
        public bool IsTestEntity { get; set; }
    }
}