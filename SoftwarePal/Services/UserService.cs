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

        public  User AddUser(User user)
        {
            return  _userRepository.Add(user);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return  _userRepository.GetAll();
        }

        public async Task<User> GetUserById(string id)
        {
            return await _userRepository.GetById(id);
        }

        public  User UpdateUser(User user)
        {
            return  _userRepository.Update(user);
        }

        public void DeleteUser(User user)
        {
            _userRepository.Delete(user);
        }

        public User GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email);
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
        User AddUser(User user);
        IEnumerable<User> GetAllUsers();
        Task<User> GetUserById(string id);
        User UpdateUser(User user);
        void DeleteUser(User user);
        User GetUserByEmail(string email);
        Task<bool> VerifyPassword(User user, string password);
        string GenerateToken(User user);
        Task<User> GetCurrentUser(ClaimsPrincipal user);
        Task SaveChanges();
    }
}
