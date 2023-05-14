using SoftwarePal.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SoftwarePal.Data;
using System.Security.Claims;
using System.Text;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace SoftwarePal.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IConfiguration _config;
        public UserRepository(ApplicationDBContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public User Add(User user)
        {
            user.Id = Guid.NewGuid().ToString();
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public async Task<User> GetById(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public User Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChangesAsync();
            return user;
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }
        public async Task<bool> VerifyPassword(User user, string password)
        {
            var dbUser = await GetById(user.Id);
            if (dbUser != null && password == dbUser.Password)
            {
                return true;
            }
            return false;
        }
        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"],
                audience: _config["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User> GetCurrentUser(ClaimsPrincipal userClaims)
        {
            string? userId = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("Unable to retrieve user ID from claims");
            }

            return await GetById(userId);
        }

        async Task IUserRepository.SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }

    public interface IUserRepository
    {
        User Add(User user);
        IEnumerable<User> GetAll();
        Task<User> GetById(string id);
        User Update(User user);
        void Delete(User user);
        User GetUserByEmail(string email);
        Task<bool> VerifyPassword(User user, string password);
        string GenerateToken(User user);
        Task<User> GetCurrentUser(ClaimsPrincipal user);
        Task SaveChanges();
    }
}
