namespace Competitions.Web.ViewModels.Customer
{
    using Data.Models.Customer;
    using Domain.BL.Mapping;

    public class CustomerViewModel : IMapFrom<Customer>
    {
        public string Mobile { get; set; }
        
        public string Email { get; set; }
        
        public ApplicationUserViewModel ApplicationUser { get; set; }
    }
}