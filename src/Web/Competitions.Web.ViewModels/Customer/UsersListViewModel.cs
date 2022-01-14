namespace Competitions.Web.ViewModels.Customer
{
    using System.Collections.Generic;

    public class UsersListViewModel
    {
        public ICollection<ApplicationUserViewModel> Users { get; set; }
    }
}