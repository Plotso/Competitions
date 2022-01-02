namespace Competitions.Web.ViewModels.Sport
{
    using Data.Models;
    using Domain.Mapping.Mapping;

    public class SportViewModel : IMapFrom<Sport>
    {
        public string Name { get; set; }
    }
}