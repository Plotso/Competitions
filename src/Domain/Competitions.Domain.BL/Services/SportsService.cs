namespace Competitions.Domain.BL.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Data.Common.Repositories;
    using Data.Models;
    using Interfaces;
    using Mapping.Mapping.Single;
    using Web.ViewModels.Sport;

    public class SportsService : ISportsService
    {
        private readonly IDeletableEntityRepository<Sport> _sportsRepository;
        //private readonly IMapper _mapper;

        public SportsService(IDeletableEntityRepository<Sport> sportsRepository)//, IMapper mapper)
        {
            _sportsRepository = sportsRepository;
            //_mapper = mapper;
        }

        public T GetById<T>(int sportId) => _sportsRepository.All().Where(s => s.Id == sportId).To<T>().FirstOrDefault();

        public IEnumerable<T> GetAll<T>()
        {
            var sports = _sportsRepository.All().Where(s => s.IsVerified);
            return sports.To<T>();
        }

        public IEnumerable<T> GetCompleteListAdmin<T>() => _sportsRepository.All().To<T>(); // ToDo: Move this to admin area

        public async Task CreateAsync(SportCreateInputModel inputModel)
        {
            var sport = new Sport
            {
                Name = inputModel.Name,
                IsVerified = inputModel.IsVerified
            };

            await _sportsRepository.AddAsync(sport);
            await _sportsRepository.SaveChangesAsync();
        }

        public async Task EditAsync(SportModifyInputModel inputModel)
        {
            var sport = GetById(inputModel.Id);
            if (sport != null)
            {
                sport.Name = inputModel.Name;
                sport.IsVerified = inputModel.IsVerified;
                
                _sportsRepository.Update(sport);
                await _sportsRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var sport = GetById(id);
            if (sport != null)
            {
                _sportsRepository.Delete(sport);
                await _sportsRepository.SaveChangesAsync();
            }
        }

        private Sport GetById(int id) => _sportsRepository.All().FirstOrDefault(s => s.Id == id);
    }
}