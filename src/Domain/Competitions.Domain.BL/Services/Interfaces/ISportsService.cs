namespace Competitions.Domain.BL.Services.Interfaces
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Web.ViewModels.Sport;

    public interface ISportsService
    {
        T GetById<T>(int sportId);

        IEnumerable<T> GetAll<T>();

        /// <summary>
        /// Get all sports including non-verified ones.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetCompleteListAdmin<T>();

        Task CreateAsync(SportCreateInputModel inputModel);

        Task EditAsync(SportModifyInputModel inputModel);

        Task DeleteAsync(int id);
    }
}