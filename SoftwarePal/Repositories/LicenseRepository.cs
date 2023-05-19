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

        async Task ILicenseRepository.SaveChanges()
        {
            await _context.SaveChangesAsync();
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
    }

    public interface ILicenseRepository
    {
        Task<License> Add(License license);
        Task<IEnumerable<License>> GetAll();
        Task<License> GetById(int id);
        Task<License> Update(License license);
        void Delete(License license);
        Task SaveChanges();
        bool Exists(int id);
    }

}
