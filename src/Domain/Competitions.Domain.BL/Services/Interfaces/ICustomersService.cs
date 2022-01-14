namespace Competitions.Domain.BL.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface ICustomersService
    {
        Task CreateInternalCustomer(string applicationUserId);

        string GetOrganiserId(string applicationUserId);

        string GetParticipantId(string applicationUserId);

        string GetCustomerId(string applicationUserId);

        T GetParticipant<T>(string participantId);

        T GetOrganiserByParticipantId<T>(string participantId);
    }
}