namespace Competitions.Data.Models.Partner
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common.Models;
    using Common.Models.Interfaces;

    public class Ground : BaseDeletableModel<int>, ITestableEntity
    {
        public string Location { get; set; }
        
        [Required]
        public int? PartnerId { get; set; }

        public virtual Partner Partner { get; set; }
        
        public int OpeningHour { get; set; }
        
        public int OpeningMinutes { get; set; }
        
        public int ClosingHour { get; set; }
        
        public int ClosingMinutes { get; set; }
        
        public virtual ICollection<Reservation> Reservations { get; set; }
        
        public bool IsTestEntity { get; set; }
    }
}