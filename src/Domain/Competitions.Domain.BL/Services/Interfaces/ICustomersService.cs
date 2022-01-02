namespace Competitions.Domain.BL.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface ICustomersService
    {
        Task CreateInternalCustomer(string applicationUserId);

        string GetOrganiserId(string applicationUserId);
    }
}