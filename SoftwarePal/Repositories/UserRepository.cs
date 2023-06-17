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

        public async Task<User> Add(User user)
        {
            user.Id = Guid.NewGuid().ToString();
            await _context.Users.AddAsync(user);
            _context.SaveChanges();
            return user;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetById(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new InvalidOperationException($"User with ID {id} not found.");
            }
            return user;
        }

        public async Task<User> Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }

        public void Delete(User user)
        {
            var dbUser = _context.Users.Find(user.Id);
            if (dbUser == null)
                throw new ArgumentNullException("User not found");
            dbUser.Status = false;
            _context.Entry(dbUser).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                throw new ArgumentNullException($"User with Email: {email} not found.");
            }
            return user;
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
                new Claim(ClaimTypes.Role.Split('/').Last(), user.UserRole.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"],
                audience: _config["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
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
        public bool Exists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        public bool EmailExists(string email)
        {
            return _context.Users.Any(u =>  u.Email == email);
        }
    }

    public interface IUserRepository
    {
        Task<User> Add(User user);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(string id);
        Task<User> Update(User user);
        void Delete(User user);
        Task<User> GetUserByEmail(string email);
        Task<bool> VerifyPassword(User user, string password);
        string GenerateToken(User user);
        Task<User> GetCurrentUser(ClaimsPrincipal user);
        bool Exists(string userId);
        bool EmailExists(string email);
    }
}
