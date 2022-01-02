namespace Competitions.Data.Models.Customer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common.Models;
    using Competition;
    using Rating;

    public class Organiser : BaseDeletableModel<string>
    {
        public Organiser()
        {
            Id = Guid.NewGuid().ToString();
        }
        
        [Required]
        public string CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        
        public virtual ICollection<Competition> Competitions { get; set; }
        
        public virtual ICollection<OrganiserRating> Ratings { get; set; }
    }
}