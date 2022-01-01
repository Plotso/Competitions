namespace Competitions.Web.ViewModels.Customer
{
    using Data.Models;
    using Domain.BL.Mapping;

    public class ApplicationUserViewModel : IMapFrom<ApplicationUser>
    {
        public string UserName { get; set; }
    }
}