using SoftwarePal.Data;
using SoftwarePal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftwarePal.Repositories
{
    public class LicenseRepository : ILicenseRepository
    {
        private readonly ApplicationDBContext _context;
        public LicenseRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<License> Add(License license)
        {
            await _context.Licenses.AddAsync(license);
            _context.SaveChanges();
            return license;
        }

        public void Delete(License license)
        {
            _context.Licenses.Remove(license);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<License>> GetAll()
        {
            return await _context.Licenses.ToListAsync();
        }

        public async Task<License> GetById(int id)
        {
            var license = await _context.Licenses.FindAsync(id);
            if (license == null)
            {
                throw new ArgumentNullException($"License with ID {id} not found.");
            }
            return license;

        }
        public async Task<License> Update(License license)
        {
            _context.Entry(license).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return license;
        }
        public bool Exists(int id)
        {
            return _context.Licenses.Any(e => e.Id == id);
        }

        public async Task<License> GetByItemId(int itemId)
        {
            var license = await _context.Licenses.FirstOrDefaultAsync(l => l.ItemId == itemId);
            if (license == null)
                throw new ArgumentNullException($"No License is found with this ItemId {itemId}");
            return license;
        }

        public async Task<List<License>> GetMyLicenses(string id)
        {
            return await _context.Licenses.Where(l => l.UserPurchased == id).ToListAsync();
        }
    }

    public interface ILicenseRepository
    {
        Task<License> Add(License license);
        Task<IEnumerable<License>> GetAll();
        Task<License> GetById(int id);
        Task<License> Update(License license);
        void Delete(License license);
        bool Exists(int id);
        Task<License> GetByItemId(int itemId);
        Task<List<License>> GetMyLicenses(string id);
    }

}
