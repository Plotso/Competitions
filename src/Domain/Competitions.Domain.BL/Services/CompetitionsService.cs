namespace Competitions.Domain.BL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Data.Common.Repositories;
    using Data.Models;
    using Data.Models.Competition;
    using Enums;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Web.ViewModels.Competition;

    public class CompetitionsService : ICompetitionsService
    {
        private readonly IDeletableEntityRepository<Competition> _competitionsRepository;
        private readonly IRepository<Sport> _sportsRepository;
        private readonly ILogger<CompetitionsService> _logger;
        private readonly IMapper _mapper;

        public CompetitionsService(IDeletableEntityRepository<Competition> competitionsRepository, IRepository<Sport> sportsRepository,ILogger<CompetitionsService> logger, IMapper mapper)
        {
            _competitionsRepository = competitionsRepository;
            _sportsRepository = sportsRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public T GetById<T>(int competitionId) 
            => _mapper.Map<T>(GetById(competitionId));

        public IEnumerable<T> GetAll<T>() 
            => _competitionsRepository.All().Select(c => _mapper.Map<T>(c));

        public IEnumerable<T> GetAllByStatus<T>(CompetitionStatus status)
        {
            var filter = GetFilterByStatus(status);
            var competitions = _competitionsRepository.All().AsEnumerable().Where(filter);
            return competitions.Select(c => _mapper.Map<T>(c));
        }

        public IEnumerable<T> GetAllBySport<T>(int sportId) 
            => _competitionsRepository.All().Where(c => c.SportId == sportId).Select(c => _mapper.Map<T>(c));

        public IEnumerable<T> GetAllBySportAndStatus<T>(int sportId, CompetitionStatus status)
        {
            var statusFilter = GetFilterByStatus(status);
            var competitions = _competitionsRepository.All().AsEnumerable().Where(c => c.SportId == sportId && statusFilter.Invoke(c));
            return competitions.Select(c => _mapper.Map<T>(c));
        }

        public IEnumerable<T> GetAllByOrganiserAndStatus<T>(string organiserId, CompetitionStatus status)
        {
            var statusFilter = GetFilterByStatus(status);
            var competitions = _competitionsRepository.All().AsEnumerable().Where(c => c.OrganiserId == organiserId && statusFilter.Invoke(c));
            return competitions.Select(c => _mapper.Map<T>(c));
        }

        public IEnumerable<T> GetAllByTypeAndStatus<T>(CompetitionType type, CompetitionStatus status)
        {
            var statusFilter = GetFilterByStatus(status);
            var competitions = _competitionsRepository.All().AsEnumerable().Where(c => c.Type == type && statusFilter.Invoke(c));
            return competitions.Select(c => _mapper.Map<T>(c));
        }

        public async Task CreateAsync(CompetitionCreateInputModel inputModel)
        {
            var competitionUpdate = inputModel.Competition;
            var competition = new Competition
            {
                Starting = competitionUpdate.Starting,
                Ending = competitionUpdate.Ending,
                EntryFee = competitionUpdate.EntryFee,
                WinningPrize = competitionUpdate.WinningPrize,
                WinPoints = competitionUpdate.WinPoints,
                DrawPoints = competitionUpdate.DrawPoints,
                CloseLosePoints = competitionUpdate.CloseLosePoints,
                IsTeamCompetition = competitionUpdate.IsTeamCompetition,
                Title = competitionUpdate.Title,
                Information = competitionUpdate.Information,
                Rules = competitionUpdate.Rules,
                Type = (CompetitionType)((int)competitionUpdate.Type)
            };

            var sportId = int.Parse(inputModel.Competition.SportId);
            var sport = _sportsRepository.All().FirstOrDefault(s => s.Id == sportId);
            if (sport == null)
            {
                var inputJson = JsonConvert.SerializeObject(inputModel, Formatting.Indented);
                _logger.LogError("Couldn't create competition due to missing sport. Request: {newLine} {inputJson}", Environment.NewLine, inputJson);
                //ToDo: Generate unique code per log?
                throw new ArgumentException(
                    "Competition creation failed. Couldn't find sport desired sportId in the database.Please try again or reach out for support.");
            }
            competition.Sport = sport;

            await _competitionsRepository.AddAsync(competition);
            await _competitionsRepository.SaveChangesAsync();
        }

        public async Task EditAsync(CompetitionModifyInputModel inputModel)
        {
            var competition = GetById(inputModel.Id);
            if (competition != null)
            {
                var competitionUpdate = inputModel.Competition;
                competition.Starting = competitionUpdate.Starting;
                competition.Ending = competitionUpdate.Ending;
                competition.EntryFee = competitionUpdate.EntryFee;
                competition.WinningPrize = competitionUpdate.WinningPrize;
                competition.WinPoints = competitionUpdate.WinPoints;
                competition.DrawPoints = competitionUpdate.DrawPoints;
                competition.CloseLosePoints = competitionUpdate.CloseLosePoints;
                competition.IsTeamCompetition = competitionUpdate.IsTeamCompetition;
                competition.Title = competitionUpdate.Title;
                competition.Information = competitionUpdate.Information;
                competition.Rules = competitionUpdate.Rules;
                competition.Type = (CompetitionType)((int)competitionUpdate.Type);

                var sportId = int.Parse(inputModel.Competition.SportId);
                var sport = _sportsRepository.All().FirstOrDefault(s => s.Id == sportId);
                if (sport != null)
                {
                    competition.Sport = sport;
                }

                _competitionsRepository.Update(competition);
                await _competitionsRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var competition = GetById(id);
            if (competition != null)
            {
                _competitionsRepository.Delete(competition);
                await _competitionsRepository.SaveChangesAsync();
            }
        }

        private Func<Competition, bool> GetFilterByStatus(CompetitionStatus status) => status switch
        {
            CompetitionStatus.Active => ActiveFilter(),
            CompetitionStatus.Upcoming => UpcomingFilter(),
            CompetitionStatus.Finished => FinishedFilter(),
            _ => (competition => true)
        };

        private Competition GetById(int id) => _competitionsRepository.All().FirstOrDefault(s => s.Id == id);

        private Func<Competition, bool> UpcomingFilter() => competition => competition.Starting > DateTime.UtcNow;
        
        private Func<Competition, bool> ActiveFilter() => competition => competition.Starting <= DateTime.UtcNow && competition.Ending > DateTime.UtcNow;
        
        private Func<Competition, bool> FinishedFilter() => competition => competition.Ending < DateTime.UtcNow;
    }
}