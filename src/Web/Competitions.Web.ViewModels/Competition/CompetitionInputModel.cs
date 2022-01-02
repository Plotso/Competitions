namespace Competitions.Web.ViewModels.Competition
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Data.Models.Competition;
    using Domain.Mapping.Mapping;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CompetitionInputModel : IMapFrom<Competition>, IMapTo<Competition>, IValidatableObject
    {
        [Required]
        [MinLength(2)]
        public string Title { get; set; }
        
        public string Rules { get; set; }
        
        [Required]
        [MinLength(15)]
        public string Information { get; set; }
        
        public CompetitionTypeViewModel Type { get; set; }
        
        [Required]
        public string TypeId { get; set; }
        
        public bool IsTeamCompetition { get; set; }
        
        public decimal? EntryFee { get; set; }
        
        public decimal? WinningPrize { get; set; }
        
        public double? WinPoints { get; set; }
        
        public double? DrawPoints { get; set; }
        
        public double? CloseLosePoints { get; set; }
        
        [Required]
        public DateTime Starting { get; set; }
        
        [Required]
        public DateTime Ending { get; set; }
        
        [Required]
        [MinLength(2)]
        public string Location { get; set; }
        
        [Required]
        public string SportId { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Ending <= Starting)
            {
                yield return new ValidationResult("Датата за край на турнира трябва да бъде след датата за начало");
            }
        }
    }
}