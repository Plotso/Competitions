namespace Competitions.Web.ViewModels.Sport
{
    using Data.Models;
    using Domain.Mapping.Mapping;

    public class SportModifyInputModel : SportCreateInputModel, IMapFrom<Sport>
    {
        public int Id { get; set; }
    }
}