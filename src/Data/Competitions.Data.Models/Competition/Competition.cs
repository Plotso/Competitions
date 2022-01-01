namespace Competitions.Data.Models.Competition
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common.Models;
    using Customer;
    using Partner;
    using Rating;

    public class Competition : BaseDeletableModel<int>
    {
        [Required]
        public string Title { get; set; }
        
        public string Rules { get; set; }
        
        public string Information { get; set; }
        
        public CompetitionType Type { get; set; }
        
        public bool IsTeamCompetition { get; set; }
        
        public decimal? EntryFee { get; set; }
        
        public decimal? WinningPrize { get; set; }
        
        public double? WinPoints { get; set; }
        
        public double? DrawPoints { get; set; }
        
        public double? CloseLosePoints { get; set; }
        
        public DateTime Starting { get; set; }
        
        public DateTime Ending { get; set; }
        
        [Required]
        public string Location { get; set; }
        
        [Required]
        public int SportId { get; set; }
        
        public virtual Sport Sport { get; set; }
        
        [Required]
        public string OrganiserId { get; set; }
        
        public virtual Organiser Organiser { get; set; }

        public virtual ICollection<CompetitionParticipant> Participants { get; set; }
        
        public virtual ICollection<Reservation> Reservations { get; set; }
        
        public virtual ICollection<CompetitionRating> Ratings { get; set; }
    }
}