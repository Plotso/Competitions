namespace Competitions.Web.ViewModels.Team
{
    using System.ComponentModel.DataAnnotations;
    using Data.Models.Team;
    using Domain.Mapping.Mapping;

    public class TeamInputModel : IMapFrom<Team>, IMapTo<Team>
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }
    }
}