namespace Competitions.Web.ViewModels.Competition
{
    using System;
    using System.Collections.Generic;
    using Customer;
    using Data.Models.Competition;
    using Domain.Mapping.Mapping;
    using Rating;
    using Sport;

    public class CompetitionViewModel : IMapFrom<Competition>
    {
        public string Title { get; set; }
        
        public string Rules { get; set; }
        
        public string Information { get; set; }
        
        public CompetitionTypeViewModel Type { get; set; }
        
        public bool IsTeamCompetition { get; set; }
        
        public decimal? EntryFee { get; set; }
        
        public decimal? WinningPrize { get; set; }
        
        public double? WinPoints { get; set; }
        
        public double? DrawPoints { get; set; }
        
        public double? CloseLosePoints { get; set; }
        
        public string Location { get; set; }
        
        public DateTime Starting { get; set; }
        
        public DateTime Ending { get; set; }
        
        public SportViewModel Sport { get; set; }
        
        public OrganiserViewModel Organiser { get; set; }
        
        public ICollection<CompetitionRatingViewModel> Ratings { get; set; }
    }
}