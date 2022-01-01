namespace Competitions.Domain.BL.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data.Models.Competition;
    using Enums;
    using Web.ViewModels.Competition;

    public interface ICompetitionsService
    {
        T GetById<T>(int competitionId);
        
        IEnumerable<T> GetAll<T>();
        
        IEnumerable<T> GetAllByStatus<T>(CompetitionStatus status);
        
        IEnumerable<T> GetAllBySport<T>(int sportId);
        
        IEnumerable<T> GetAllBySportAndStatus<T>(int sportId, CompetitionStatus status);
        
        IEnumerable<T> GetAllByOrganiserAndStatus<T>(string organiserId, CompetitionStatus status);

        IEnumerable<T> GetAllByTypeAndStatus<T>(CompetitionType type, CompetitionStatus status);

        Task CreateAsync(CompetitionCreateInputModel inputModel);

        Task EditAsync(CompetitionModifyInputModel inputModel);

        Task DeleteAsync(int id);
    }
}