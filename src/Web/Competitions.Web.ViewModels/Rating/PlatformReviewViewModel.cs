namespace Competitions.Web.ViewModels.Rating
{
    using System.ComponentModel.DataAnnotations;
    using Data.Models.Rating;
    using Domain.Mapping.Mapping;

    public class PlatformReviewViewModel : IMapFrom<PlatformReview>, IMapTo<PlatformReview>
    {
        [Range(0.0, 5.0)]
        public double Score { get; set; }
        
        [Required]
        public string Comment { get; set; }
    }
}