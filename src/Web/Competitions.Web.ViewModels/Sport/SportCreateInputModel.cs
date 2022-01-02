namespace Competitions.Web.ViewModels.Sport
{
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
    using Domain.Mapping.Mapping;

    public class SportCreateInputModel : IMapTo<Sport>
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        
        public bool IsVerified { get; set; }
    }
}