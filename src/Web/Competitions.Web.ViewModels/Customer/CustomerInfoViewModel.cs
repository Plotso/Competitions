namespace Competitions.Web.ViewModels.Customer
{
    using System.Collections.Generic;
    using Competition;
    using Team;

    public class CustomerInfoViewModel
    {
        public IEnumerable<TeamViewModel> Teams { get; set; }
        public IEnumerable<CompetitionViewModel> TeamCompetitions { get; set; }
        public IEnumerable<CompetitionViewModel> IndividualCompetitions { get; set; }
        
        public IEnumerable<CompetitionViewModel> OrganisedCompetitions { get; set; }
    }
}