namespace Competitions.Web.ViewModels.Customer
{
    using System.Collections.Generic;
    using Competition;
    using Data.Models.Customer;
    using Domain.Mapping.Mapping;
    using Team;

    public class ParticipantViewModel : IMapFrom<Participant>
    {
        public CustomerViewModel Customer { get; set; }
        
        public ICollection<CompetitionParticipantViewModel> Competitions { get; set; }
        
        public ICollection<TeamParticipantViewModel> Teams { get; set; }
    }
}