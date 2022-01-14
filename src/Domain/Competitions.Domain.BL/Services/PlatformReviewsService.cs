namespace Competitions.Domain.BL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Common.Repositories;
    using Data.Models.Customer;
    using Data.Models.Rating;
    using Interfaces;
    using Mapping.Mapping.Single;
    using Web.ViewModels.Rating;

    public class PlatformReviewsService : IPlatformReviewsService
    {
        private readonly IDeletableEntityRepository<PlatformReview> _platformReviewsRepository;
        private readonly IRepository<Customer> _customersRepository;

        public PlatformReviewsService(IDeletableEntityRepository<PlatformReview> platformReviewsRepository, IRepository<Customer> customersRepository)
        {
            _platformReviewsRepository = platformReviewsRepository;
            _customersRepository = customersRepository;
        }

        public T GetById<T>(int reviewId) 
            => _platformReviewsRepository.All().Where(r => r.Id == reviewId).To<T>().FirstOrDefault();

        public IEnumerable<T> GetAll<T>() 
            => _platformReviewsRepository.All().To<T>();

        public async Task CreateAsync(PlatformReviewViewModel inputModel, string customerId)
        {
            var customer = _customersRepository.All().FirstOrDefault(c => c.Id == customerId);
            if (customer == null)
                throw new ArgumentException("Non-existing customer id provided!!");

            var review = new PlatformReview
            {
                Score = inputModel.Score,
                Comment = inputModel.Comment,
                Customer = customer
            };

            await _platformReviewsRepository.AddAsync(review);
            await _platformReviewsRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var review = _platformReviewsRepository.All().FirstOrDefault(r => r.Id == id);
            if (review == null)
                throw new ArgumentException("Delete review failed! Non-existing review id provided!!");
            
            _platformReviewsRepository.Delete(review);
            await _platformReviewsRepository.SaveChangesAsync();
        }
    }
}