using SoftwarePal.Models;
using SoftwarePal.Repositories;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace SoftwarePal.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TeamService(ITeamRepository teamRepository, IHttpContextAccessor httpContextAccessor)
        {
            _teamRepository = teamRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Team> Add(Team team)
        {
            team.CreatedAt = DateTime.Now;
            return await _teamRepository.Add(team);
        }

        public void Delete(Team team)
        {
            if (!_teamRepository.Exists(team.Id))
                throw new InvalidOperationException($"Team with ID {team.Id} not found.");
            _teamRepository.Delete(team);
        }

        public Task<IEnumerable<Team>> GetAll()
        {
            return _teamRepository.GetAll();
        }

        public async Task<Team> GetById(int id)
        {
            var team = await _teamRepository.GetById(id);
            string origin = GetAppOrigin();
            if (team.ImageName != null)
                team.ImageName = origin + "/" + team.ImageName;
            return team;
        }

        public Task SaveChanges()
        {
            return _teamRepository.SaveChanges();
        }

        public async Task<Team> Update(Team team)
        {
            if (!_teamRepository.Exists(team.Id))
                throw new InvalidOperationException($"Team with ID {team.Id} not found.");
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
                    var fullPath = Path.Combine(path, "Team-" + Guid.NewGuid() + "." + image.ContentType.Split('/').Last());
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                    return fullPath.Substring(fullPath.IndexOf("Images")).Replace("\\", "/");
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
        public string GetAppOrigin()
        {
            var request = _httpContextAccessor.HttpContext?.Request;

            var origin = $"{request?.Scheme}://{request?.Host}";

            return origin;
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
        string GetAppOrigin();
    }
}
