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
        public async Task<string> SaveImage(IFormFile image)
        {
            string path = "";
            try
            {
                if (image.Length > 0)
                {
                    path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "Images\\Team"));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var fullPath = Path.Combine(path, "Team-" + Guid.NewGuid());
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                    return fullPath;
                }
                else
                {
                    throw new Exception("File empty");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("File Copy Failed", ex);
            }
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
        Task<string> SaveImage(IFormFile image);
    }
}
