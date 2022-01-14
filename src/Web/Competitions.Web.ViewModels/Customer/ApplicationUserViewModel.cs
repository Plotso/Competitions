namespace Competitions.Web.ViewModels.Customer
{
    using System;
    using Data.Models;
    using Domain.Mapping.Mapping;

    public class ApplicationUserViewModel : IMapFrom<ApplicationUser>
    {
        public string UserName { get; set; }
        
        public string Email { get; set; }
        
        public bool IsAdmin { get; set; }
        
        public bool IsDeleted { get; set; }
        
        public DateTime CreatedOn { get; set; }
        
        public DateTime DeletedOn { get; set; }
    }
}