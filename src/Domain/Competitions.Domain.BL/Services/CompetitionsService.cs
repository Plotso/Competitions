namespace Competitions.Domain.BL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Common.Repositories;
    using Data.Models;
    using Data.Models.Competition;
    using Data.Models.Customer;
    using Enums;
    using Exceptions;
    using Interfaces;
    using Mapping.Mapping.Single;
    using Microsoft.Extensions.Logging;
    using Web.ViewModels.Competition;

    public class CompetitionsService : ICompetitionsService
    {
        private readonly IDeletableEntityRepository<Competition> _competitionsRepository;
        private readonly IDeletableEntityRepository<CompetitionParticipant> _competitionParticipantsMappingRepository;
        private readonly IRepository<Sport> _sportsRepository;
        private readonly IRepository<Organiser> _organisersRepository;
        private readonly IRepository<Participant> _participantsRepository;
        private readonly ILogger<CompetitionsService> _logger;

        public CompetitionsService(
            IDeletableEntityRepository<Competition> competitionsRepository,
            IDeletableEntityRepository<CompetitionParticipant> competitionParticipantsMappingRepository,
            IRepository<Sport> sportsRepository,
            IRepository<Organiser> organisersRepository,
            IRepository<Participant> participantsRepository,
            ILogger<CompetitionsService> logger)
        {
            _competitionsRepository = competitionsRepository;
            _competitionParticipantsMappingRepository = competitionParticipantsMappingRepository;
            _sportsRepository = sportsRepository;
            _organisersRepository = organisersRepository;
            _participantsRepository = participantsRepository;
            _logger = logger;
        }

        public T GetById<T>(int competitionId) => Competitions.Where(c => c.Id == competitionId).To<T>().FirstOrDefault();

        public IEnumerable<T> GetAll<T>() 
            => Competitions.To<T>();

        public IEnumerable<T> GetAllByStatus<T>(CompetitionStatus status)
        {
            var filter = GetFilterByStatus(status);
            var competitions = Competitions.AsEnumerable().Where(filter).AsQueryable();
            return competitions.To<T>();
        }

        public IEnumerable<T> GetAllBySport<T>(int sportId) 
            => _competitionsRepository.All().Where(c => c.SportId == sportId).To<T>();

        public IEnumerable<T> GetAllBySportAndStatus<T>(int sportId, CompetitionStatus status)
        {
            var statusFilter = GetFilterByStatus(status);
            var competitions = Competitions.AsEnumerable().Where(c => c.SportId == sportId && statusFilter.Invoke(c)).AsQueryable();
            return competitions.To<T>();
        }

        public IEnumerable<T> GetAllBySportAndStatuses<T>(int sportId, params CompetitionStatus[] statuses)
        {
            var filters = statuses.Select(GetFilterByStatus);
            var competitions = Competitions.AsEnumerable().Where(c => c.SportId == sportId && (filters.Any(f => f.Invoke(c)))).AsQueryable();
            return competitions.To<T>();
        }

        public IEnumerable<T> GetAllByOrganiserAndStatus<T>(string organiserId, CompetitionStatus status)
        {
            var statusFilter = GetFilterByStatus(status);
            var competitions = Competitions.AsEnumerable().Where(c => c.OrganiserId == organiserId && statusFilter.Invoke(c)).AsQueryable();
            return competitions.To<T>();
        }

        public IEnumerable<T> GetAllByTypeAndStatus<T>(CompetitionType type, CompetitionStatus status)
        {
            var statusFilter = GetFilterByStatus(status);
            var competitions = Competitions.AsEnumerable().Where(c => c.Type == type && statusFilter.Invoke(c)).AsQueryable();
            return competitions.To<T>();
        }

        public async Task SignParticipant(int competitionId, string participantId, int? teamId = null)
        {
            var competition = GetById(competitionId);
            if (competition == null)
                throw new ArgumentException($"Couldn't find desire Competition with ID: {competitionId}");
            var participant = _participantsRepository.All().FirstOrDefault(p => p.Id == participantId);
            if (participant == null)
                throw new ArgumentException($"Competition participation failed due to missing participant from database. Provided id: {participantId}");
                
            var competitionParticipant = new CompetitionParticipant
            {
                Competition = competition
            };
            if (competition.IsTeamCompetition)
            {
                if (participant.Teams == null || !participant.Teams.Any())
                {
                    throw new ArgumentException($"Participant with id: {participantId} doesn't have any active teams. Competition {competitionId} is marked as teams competition only!");
                }

                if (teamId == null && participant.Teams.Count > 1)
                {
                    throw new ArgumentException($"Participant with id: {participantId} hasn't chosen which team to participate with!");
                }

                var team = teamId == null ? 
                    participant.Teams.FirstOrDefault() :
                    participant.Teams.FirstOrDefault(t => t.TeamId == teamId);
                if (team == null)
                {
                    throw new ArgumentException($"Participant with id: {participantId} isn't associated with team with id {teamId}");
                }
                    
                competitionParticipant.Team = team.Team;
            }
            else
            {
                competitionParticipant.Participant = participant;
            }

            await _competitionParticipantsMappingRepository.AddAsync(competitionParticipant);
            await _competitionParticipantsMappingRepository.SaveChangesAsync();
        }

        public async Task UnSignParticipant(int competitionId, string participantId, int? teamId = null)
        {
            var competition = GetById(competitionId);
            if (competition == null)
                throw new ArgumentException($"Couldn't find desire Competition with ID: {competitionId}");
            var participant = _participantsRepository.All().FirstOrDefault(p => p.Id == participantId);
            if (participant == null)
                throw new ArgumentException($"Competition participation failed due to missing participant from database. Provided id: {participantId}");

            CompetitionParticipant competitionParticipant;
            if (competition.IsTeamCompetition)
            {
                if (participant.Teams == null || !participant.Teams.Any())
                {
                    throw new ArgumentException($"Participant with id: {participantId} doesn't have any active teams. Competition {competitionId} is marked as teams competition only! UnSign failed!");
                }

                var team = participant.Teams.FirstOrDefault(t =>
                    t.Team.Competitions.Any(c => c.CompetitionId == competitionId));
                if (team == null)
                {
                    throw new ArgumentException($"Participant with id: {participantId} isn't associated with team with id {teamId}! UnSign failed!");
                }

                competitionParticipant = _competitionParticipantsMappingRepository.All()
                    .FirstOrDefault(cp => cp.TeamId == team.Id && cp.CompetitionId == competitionId);
            }
            else
            {
                competitionParticipant = _competitionParticipantsMappingRepository.All()
                    .FirstOrDefault(cp => cp.ParticipantId == participant.Id && cp.CompetitionId == competitionId);
            }

            if (competitionParticipant == null)
                throw new ArgumentException($"Couldn't find any competition participant with provided data: CompetitionId: {competitionId}, ParticipantId: {participantId}, TeamId: {teamId}");

            _competitionParticipantsMappingRepository.Delete(competitionParticipant);
            await _competitionsRepository.SaveChangesAsync();
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
            throw new ArgumentException($"Edit competition failed. Competition with ID: {inputModel.Id} couldn't be found in the database");
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

        private Competition GetById(int id) => Competitions.FirstOrDefault(s => s.Id == id);
        private IQueryable<Competition> Competitions => _competitionsRepository.All();

        private Func<Competition, bool> UpcomingFilter() => competition => competition.Starting > DateTime.UtcNow;
        
        private Func<Competition, bool> ActiveFilter() => competition => competition.Starting <= DateTime.UtcNow && competition.Ending > DateTime.UtcNow;
        
        private Func<Competition, bool> FinishedFilter() => competition => competition.Ending < DateTime.UtcNow;
    }
}