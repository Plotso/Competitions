namespace Competitions.Web.ViewModels.Customer
{
    using System.Collections.Generic;
    using Data.Models.Customer;
    using Domain.Mapping.Mapping;
    using Rating;

    public class OrganiserViewModel : IMapFrom<Organiser>
    {
        public CustomerViewModel Customer { get; set; }
        
        public ICollection<OrganiserRatingViewModel> Ratings { get; set; }
    }
}