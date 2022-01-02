namespace Competitions.Web.ViewModels.Sport
{
    using Data.Models;
    using Domain.Mapping.Mapping;

    public class SportViewModel : IMapFrom<Sport>
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
    }
}