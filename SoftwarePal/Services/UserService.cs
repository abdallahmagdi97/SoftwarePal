using SoftwarePal.Models;
using SoftwarePal.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SoftwarePal.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AddUser(User user)
        {
            user.CreatedAt = DateTime.Now;
            return await _userRepository.Add(user);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userRepository.GetAll();
        }

        public async Task<User> GetUserById(string id)
        {
            if (!_userRepository.Exists(id))
            {
                throw new Exception("Not Found");
            }
            return await _userRepository.GetById(id);
        }

        public async Task<User> UpdateUser(User user)
        {
            if (!_userRepository.Exists(user.Id))
                throw new InvalidOperationException($"User with ID {user.Id} not found.");
            return await _userRepository.Update(user);
        }

        public void DeleteUser(User user)
        {
            if (!_userRepository.Exists(user.Id))
                throw new InvalidOperationException($"User with ID {user.Id} not found.");
            _userRepository.Delete(user);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }

        public async Task<bool> VerifyPassword(User user, string password)
        {
            return await _userRepository.VerifyPassword(user, password);
        }

        public  string GenerateToken(User user)
        {
            return  _userRepository.GenerateToken(user);
        }

        public async Task<User> GetCurrentUser(ClaimsPrincipal user)
        {
            return await _userRepository.GetCurrentUser(user);
        }

        async Task IUserService.SaveChanges()
        {
            await _userRepository.SaveChanges();
        }
    }

    public interface IUserService
    {
        Task<User> AddUser(User user);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(string id);
        Task<User> UpdateUser(User user);
        void DeleteUser(User user);
        Task<User> GetUserByEmail(string email);
        Task<bool> VerifyPassword(User user, string password);
        string GenerateToken(User user);
        Task<User> GetCurrentUser(ClaimsPrincipal user);
        Task SaveChanges();
    }
}
