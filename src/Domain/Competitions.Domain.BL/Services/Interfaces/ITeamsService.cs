namespace Competitions.Domain.BL.Services.Interfaces
{
    using System.Threading.Tasks;
    using Web.ViewModels.Team;

    public interface ITeamsService
    {
        
        T GetById<T>(int teamId);

        Task<int> CreateAsync(TeamInputModel inputModel, string customerId);

        Task EditAsync(TeamModifyInputModel inputModel);

        Task DeleteAsync(int teamId);
    }
}