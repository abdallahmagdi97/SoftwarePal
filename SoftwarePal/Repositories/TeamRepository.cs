using SoftwarePal.Data;
using SoftwarePal.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace SoftwarePal.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationDBContext _context;
        public TeamRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Team> Add(Team team)
        {
            await _context.Teams.AddAsync(team);
            _context.SaveChanges();
            return team;
        }

        public void Delete(Team team)
        {
            _context.Teams.Remove(team);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Team>> GetAll()
        {
            return await _context.Teams.ToListAsync();
        }

        public async Task<Team> GetById(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                throw new InvalidOperationException($"Team with ID {id} not found.");
            }
            return team;

        }

        async Task ITeamRepository.SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Team> Update(Team team)
        {
            _context.Entry(team).State = EntityState.Modified;
            if (team.ImageName == null)
            {
                _context.Entry(team).Property(nameof(team.ImageName)).IsModified = false;
            }
            await _context.SaveChangesAsync();
            return team;
        }
    }

    public interface ITeamRepository
    {
        Task<Team> Add(Team team);
        Task<IEnumerable<Team>> GetAll();
        Task<Team> GetById(int id);
        Task<Team> Update(Team team);
        void Delete(Team team);
        Task SaveChanges();
    }
}
