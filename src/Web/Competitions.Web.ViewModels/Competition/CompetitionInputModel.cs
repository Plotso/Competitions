namespace Competitions.Web.ViewModels.Competition
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Data.Models.Competition;
    using Domain.Mapping.Mapping;

    public class CompetitionInputModel : IMapFrom<Competition>, IMapTo<Competition>
    {
        [Required]
        [MinLength(2)]
        public string Title { get; set; }
        
        public string Rules { get; set; }
        
        [Required]
        [MinLength(15)]
        public string Information { get; set; }
        
        public CompetitionTypeViewModel Type { get; set; }
        
        public bool IsTeamCompetition { get; set; }
        
        public decimal? EntryFee { get; set; }
        
        public decimal? WinningPrize { get; set; }
        
        public double? WinPoints { get; set; }
        
        public double? DrawPoints { get; set; }
        
        public double? CloseLosePoints { get; set; }
        
        public DateTime Starting { get; set; }
        
        public DateTime Ending { get; set; }
        
        [Required]
        [MinLength(2)]
        public string Location { get; set; }
        
        public string SportId { get; set; }
    }
}