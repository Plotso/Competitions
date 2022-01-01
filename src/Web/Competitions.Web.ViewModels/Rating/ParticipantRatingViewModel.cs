namespace Competitions.Web.ViewModels.Rating
{
    using Data.Models.Rating;
    using Domain.BL.Mapping;

    public class ParticipantRatingViewModel : IMapFrom<ParticipantRating>
    {
        public double Score { get; set; }
    }
}