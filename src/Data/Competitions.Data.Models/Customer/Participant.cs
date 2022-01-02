namespace Competitions.Data.Models.Customer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common.Models;
    using Competition;
    using Team;

    public class Participant : BaseDeletableModel<string>
    {
        public Participant()
        {
            Id = Guid.NewGuid().ToString();
        }
        
        [Required]
        public string CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        
        public virtual ICollection<CompetitionParticipant> Competitions { get; set; }
        
        public virtual ICollection<TeamParticipant> Teams { get; set; }
    }
}