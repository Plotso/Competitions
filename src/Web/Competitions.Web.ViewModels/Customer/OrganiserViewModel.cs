namespace Competitions.Web.ViewModels.Customer
{
    using System.Collections.Generic;
    using Competition;
    using Data.Models.Customer;
    using Domain.Mapping.Mapping;
    using Rating;

    public class OrganiserViewModel : IMapFrom<Organiser>
    {
        public string Id { get; set; }
        
        public CustomerViewModel Customer { get; set; }
        
        public IEnumerable<OrganiserRatingViewModel> Ratings { get; set; }
        
        public IEnumerable<CompetitionViewModel> Competitions { get; set; }
    }
}