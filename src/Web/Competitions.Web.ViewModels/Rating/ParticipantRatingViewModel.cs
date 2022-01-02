namespace Competitions.Web.ViewModels.Rating
{
    using Data.Models.Rating;
    using Domain.Mapping.Mapping;

    public class ParticipantRatingViewModel : IMapFrom<ParticipantRating>
    {
        public double Score { get; set; }
    }
}