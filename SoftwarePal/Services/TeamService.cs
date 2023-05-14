using SoftwarePal.Models;
using SoftwarePal.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftwarePal.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<Team> Add(Team team)
        {
            return await _teamRepository.Add(team);
        }

        public void Delete(Team team)
        {
            _teamRepository.Delete(team);
        }

        public Task<IEnumerable<Team>> GetAll()
        {
            return _teamRepository.GetAll();
        }

        public Task<Team> GetById(int id)
        {
            return _teamRepository.GetById(id);
        }

        public Task SaveChanges()
        {
            return _teamRepository.SaveChanges();
        }

        public async Task<Team> Update(Team team)
        {
            return await _teamRepository.Update(team);
        }
    }

    public interface ITeamService
    {
        Task<Team> Add(Team team);
        Task<IEnumerable<Team>> GetAll();
        Task<Team> GetById(int id);
        Task<Team> Update(Team team);
        void Delete(Team team);
        Task SaveChanges();
    }
}
