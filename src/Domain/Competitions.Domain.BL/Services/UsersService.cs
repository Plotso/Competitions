namespace Competitions.Domain.BL.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Common.Repositories;
    using Data.Models;
    using Interfaces;
    using Mapping.Mapping.Single;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> _usersRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public ApplicationUser FindByEmail(string email) =>
            _usersRepository.AllWithDeleted().FirstOrDefault(u => u.Email == email);

        public IEnumerable<T> GetAllWithDeleted<T>() => _usersRepository.AllWithDeleted().To<T>();

        public async Task ChangeIsDelete(string email)
        {
            var user = _usersRepository.AllWithDeleted().FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                if (user.IsDeleted)
                {
                    _usersRepository.Undelete(user);
                }
                else
                {
                    _usersRepository.Delete(user);
                }

                await _usersRepository.SaveChangesAsync();
            }
        }
    }
}