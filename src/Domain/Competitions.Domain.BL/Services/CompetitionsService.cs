﻿namespace Competitions.Domain.BL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Data.Common.Repositories;
    using Data.Models;
    using Data.Models.Competition;
    using Data.Models.Customer;
    using Enums;
    using Exceptions;
    using Interfaces;
    using Mapping.Mapping.Single;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Web.ViewModels.Competition;

    public class CompetitionsService : ICompetitionsService
    {
        private readonly IDeletableEntityRepository<Competition> _competitionsRepository;
        private readonly IRepository<Sport> _sportsRepository;
        private readonly IRepository<Organiser> _organisersRepository;
        private readonly ILogger<CompetitionsService> _logger;

        public CompetitionsService(
            IDeletableEntityRepository<Competition> competitionsRepository,
            IRepository<Sport> sportsRepository,
            IRepository<Organiser> organisersRepository,
            ILogger<CompetitionsService> logger)
        {
            _competitionsRepository = competitionsRepository;
            _sportsRepository = sportsRepository;
            _organisersRepository = organisersRepository;
            _logger = logger;
        }

        public T GetById<T>(int competitionId) => _competitionsRepository.All().To<T>().FirstOrDefault();

        public IEnumerable<T> GetAll<T>() 
            => _competitionsRepository.All().To<T>();

        public IEnumerable<T> GetAllByStatus<T>(CompetitionStatus status)
        {
            var filter = GetFilterByStatus(status);
            var competitions = _competitionsRepository.All().AsEnumerable().Where(filter).AsQueryable();
            return competitions.To<T>();
        }

        public IEnumerable<T> GetAllBySport<T>(int sportId) 
            => _competitionsRepository.All().Where(c => c.SportId == sportId).To<T>();

        public IEnumerable<T> GetAllBySportAndStatus<T>(int sportId, CompetitionStatus status)
        {
            var statusFilter = GetFilterByStatus(status);
            var competitions = _competitionsRepository.All().AsEnumerable().Where(c => c.SportId == sportId && statusFilter.Invoke(c)).AsQueryable();
            return competitions.To<T>();
        }

        public IEnumerable<T> GetAllBySportAndStatuses<T>(int sportId, params CompetitionStatus[] statuses)
        {
            var filters = statuses.Select(GetFilterByStatus);
            var competitions = _competitionsRepository.All().AsEnumerable().Where(c => c.SportId == sportId && (filters.Any(f => f.Invoke(c)))).AsQueryable();
            return competitions.To<T>();
        }

        public IEnumerable<T> GetAllByOrganiserAndStatus<T>(string organiserId, CompetitionStatus status)
        {
            var statusFilter = GetFilterByStatus(status);
            var competitions = _competitionsRepository.All().AsEnumerable().Where(c => c.OrganiserId == organiserId && statusFilter.Invoke(c)).AsQueryable();
            return competitions.To<T>();
        }

        public IEnumerable<T> GetAllByTypeAndStatus<T>(CompetitionType type, CompetitionStatus status)
        {
            var statusFilter = GetFilterByStatus(status);
            var competitions = _competitionsRepository.All().AsEnumerable().Where(c => c.Type == type && statusFilter.Invoke(c)).AsQueryable();
            return competitions.To<T>();
        }

        public async Task CreateAsync(CompetitionCreateInputModel inputModel, string organiserId)
        {
            var organiser = _organisersRepository.All().FirstOrDefault(o => o.Id == organiserId);
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
                Location = competitionUpdate.Location,
                Organiser = organiser,
                //Type = (CompetitionType)((int)competitionUpdate.Type)
                Type = (CompetitionType)(int.Parse(competitionUpdate.TypeId))
            };

            var sportId = int.Parse(inputModel.Competition.SportId);
            var sport = _sportsRepository.All().FirstOrDefault(s => s.Id == sportId);
            if (sport == null)
            {
                //ToDo: Generate unique code per log?
                throw new MissingSportException(
                    $"Competition creation failed. Couldn't find desired Sport in the database.Please try again or reach out for support.");
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
        { // ToDo: Verify filters since results on site ain't correct
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