namespace Competitions.Domain.BL.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Common.Repositories;
    using Data.Models.Team;
    using Interfaces;
    using Mapping.Mapping.Single;
    using Microsoft.EntityFrameworkCore;
    using Web.ViewModels.Team;

    public class TeamsService : ITeamsService // ToDo: Add participant to team mapping creation inside here
    {
        private readonly IDeletableEntityRepository<Team> _teamsRepository;

        public TeamsService(IDeletableEntityRepository<Team> teamsRepository)
        {
            _teamsRepository = teamsRepository;
        }

        public T GetById<T>(int teamId) => Teams.Where(t => t.Id == teamId).To<T>().FirstOrDefault();

        public async Task<int> CreateAsync(TeamInputModel inputModel, string customerId)
        {
            var team = new Team {Name = inputModel.Name, CreatorCustomerId = customerId};
            await _teamsRepository.AddAsync(team);
            await _teamsRepository.SaveChangesAsync();
            return team.Id;
        }

        public async Task EditAsync(TeamModifyInputModel inputModel)
        {
            var team = await GetById(inputModel.Id);
            if (team != null)
            {
                team.Name = inputModel.Team.Name;
                
                _teamsRepository.Update(team);
                await _teamsRepository.SaveChangesAsync();
            }

            throw new ArgumentException($"Edit team failed. Team with ID: {inputModel.Id} couldn't be found in the database");
        }

        public async Task DeleteAsync(int teamId)
        {
            var team = await GetById(teamId);
            if (team != null)
            {
                
                _teamsRepository.Delete(team);
                await _teamsRepository.SaveChangesAsync();
            }
        }

        private IQueryable<Team> Teams => _teamsRepository.All();
        private async Task<Team> GetById(int id) => await Teams.FirstOrDefaultAsync(t => t.Id == id);
    }
}