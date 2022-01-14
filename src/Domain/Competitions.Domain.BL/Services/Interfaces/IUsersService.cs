namespace Competitions.Domain.BL.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data.Models;

    public interface IUsersService
    {
        ApplicationUser FindByEmail(string email);
        
        IEnumerable<T> GetAllWithDeleted<T>();
        
        Task ChangeIsDelete(string username);
    }
}