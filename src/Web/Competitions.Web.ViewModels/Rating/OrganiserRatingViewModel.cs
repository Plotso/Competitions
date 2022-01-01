namespace Competitions.Web.ViewModels.Rating
{
    using Data.Models.Rating;
    using Domain.BL.Mapping;

    public class OrganiserRatingViewModel : IMapFrom<OrganiserRating>
    {
        public double Score { get; set; }
    }
}