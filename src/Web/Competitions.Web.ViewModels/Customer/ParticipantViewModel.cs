namespace Competitions.Web.ViewModels.Customer
{
    using System.Collections.Generic;
    using Data.Models.Customer;
    using Domain.BL.Mapping;
    using Rating;

    public class ParticipantViewModel : IMapFrom<Participant>
    {
        public CustomerViewModel Customer { get; set; }
        
        public ICollection<ParticipantRatingViewModel> Ratings { get; set; }
    }
}