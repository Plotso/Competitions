namespace Competitions.Web.ViewModels.Rating
{
    using Data.Models.Rating;
    using Domain.BL.Mapping;

    public class CompetitionRatingViewModel : IMapFrom<CompetitionRating>
    {
        public double Score { get; set; }
    }
}