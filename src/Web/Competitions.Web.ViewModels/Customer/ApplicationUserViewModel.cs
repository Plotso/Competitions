namespace Competitions.Web.ViewModels.Customer
{
    using Data.Models;
    using Domain.Mapping.Mapping;

    public class ApplicationUserViewModel : IMapFrom<ApplicationUser>
    {
        public string UserName { get; set; }
    }
}