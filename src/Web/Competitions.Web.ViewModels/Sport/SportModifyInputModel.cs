namespace Competitions.Web.ViewModels.Sport
{
    using Data.Models;
    using Domain.BL.Mapping;

    public class SportModifyInputModel : SportCreateInputModel, IMapFrom<Sport>
    {
        public int Id { get; set; }
    }
}