namespace Competitions.Data.Models.Customer
{
    using System;
    using Common.Models;

    public class Customer : BaseDeletableModel<string>
    {
        public Customer()
        {
            Id = Guid.NewGuid().ToString();
        }
        
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        
        public string Mobile { get; set; }
        
        public string Email { get; set; }
        
        public virtual Participant Participant { get; set; }
        
        public virtual Organiser Organiser { get; set; }
    }
}