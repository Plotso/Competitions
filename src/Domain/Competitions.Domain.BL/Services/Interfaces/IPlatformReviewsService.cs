namespace Competitions.Domain.BL.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Web.ViewModels.Rating;

    public interface IPlatformReviewsService
    {
        T GetById<T>(int reviewId);
        
        IEnumerable<T> GetAll<T>();
        
        Task CreateAsync(PlatformReviewViewModel inputModel, string customerId);

        Task DeleteAsync(int id);
    }
}