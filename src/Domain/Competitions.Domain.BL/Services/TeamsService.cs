namespace Competitions.Domain.BL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Common.Repositories;
    using Data.Models.Customer;
    using Data.Models.Team;
    using Interfaces;
    using Mapping.Mapping.Single;
    using Microsoft.EntityFrameworkCore;
    using Web.ViewModels.Team;

    public class TeamsService : ITeamsService
    {
        private readonly IDeletableEntityRepository<Team> _teamsRepository;
        private readonly IDeletableEntityRepository<TeamParticipant> _teamParticipants;
        private readonly IRepository<Participant> _participantsRepository;

        public TeamsService(IDeletableEntityRepository<Team> teamsRepository, IDeletableEntityRepository<TeamParticipant> teamParticipants, IRepository<Participant> participantsRepository)
        {
            _teamsRepository = teamsRepository;
            _teamParticipants = teamParticipants;
            _participantsRepository = participantsRepository;
        }

        public IEnumerable<T> GetAllByParticipantId<T>(string participantId) => Teams.Where(t => t.Participants.Any(p => p.ParticipantId == participantId)).To<T>();

        public IEnumerable<T> GetAllByParticipantIdAlt<T>(string participantId)
        {
            var participant = _participantsRepository.All().FirstOrDefault(p => p.Id == participantId);
            var participantTeams = participant?.Teams.Select(pt => pt.Team);
            return participantTeams.Map<Team, T>();
        }

        public T GetById<T>(int teamId) => Teams.Where(t => t.Id == teamId).To<T>().FirstOrDefault();

        public async Task<int> CreateAsync(TeamInputModel inputModel, string customerId)
        {
            var team = new Team {Name = inputModel.Name, CreatorCustomerId = customerId};
            await _teamsRepository.AddAsync(team);
            await _teamsRepository.SaveChangesAsync();

            var participant = _participantsRepository.All().FirstOrDefault(p => p.CustomerId == customerId);
            if (participant != null)
            {
                var teamParticipant = new TeamParticipant {Team = team, Participant = participant};
                await _teamParticipants.AddAsync(teamParticipant);
                await _teamParticipants.SaveChangesAsync();
            }

            var teamId = Teams.First(t => t.Name == inputModel.Name && t.CreatorCustomerId == customerId).Id;
            return teamId;
        }

        public async Task EditAsync(TeamModifyInputModel inputModel)
        {
            var team = await GetById(inputModel.Id);
            if (team == null)
                throw new ArgumentException($"Edit team failed. Team with ID: {inputModel.Id} couldn't be found in the database");
            
            team.Name = inputModel.Team.Name;
                
            _teamsRepository.Update(team);
            await _teamsRepository.SaveChangesAsync();

        }

        public async Task DeleteAsync(int teamId)
        {
            var team = await GetById(teamId);
            if (team != null)
            {
                _teamsRepository.Delete(team);
                await _teamsRepository.SaveChangesAsync();

                var teamParticipants = _teamParticipants.All().Where(t => t.TeamId == teamId);
                foreach (var teamParticipant in teamParticipants)
                {
                    _teamParticipants.Delete(teamParticipant);
                    await _teamParticipants.SaveChangesAsync();
                }
            }
        }

        private IQueryable<Team> Teams => _teamsRepository.All();
        private async Task<Team> GetById(int id) => await Teams.FirstOrDefaultAsync(t => t.Id == id);
    }
}