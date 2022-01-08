namespace Competitions.Web.ViewModels.Team
{
    using Data.Models.Team;
    using Domain.Mapping.Mapping;

    public class TeamViewModel : IMapFrom<Team>
    {
        public string Name { get; set; }
    }
}