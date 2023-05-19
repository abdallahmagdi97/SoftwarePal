using SoftwarePal.Data;
using SoftwarePal.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SoftwarePal.Repositories
{
    public class AboutUsRepository : IAboutUsRepository
    {
        private readonly ApplicationDBContext _context;
        public AboutUsRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<AboutUs> Add(AboutUs aboutUs)
        {
            await _context.AboutUs.AddAsync(aboutUs);
            _context.SaveChanges();
            return aboutUs;
        }

        public void Delete(AboutUs aboutUs)
        {
            _context.AboutUs.Remove(aboutUs);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<AboutUs>> GetAll()
        {
            return await _context.AboutUs.ToListAsync();
        }

        public async Task<AboutUs> GetById(int id)
        {
            var subItem = await _context.AboutUs.FindAsync(id);
            if (subItem == null)
            {
                throw new InvalidOperationException($"AboutUs with ID {id} not found.");
            }
            return subItem;

        }

        async Task IAboutUsRepository.SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<AboutUs> Update(AboutUs aboutUs)
        {
            _context.Entry(aboutUs).State = EntityState.Modified;
            if (aboutUs.ImageName == null)
            {
                _context.Entry(aboutUs).Property(nameof(aboutUs.ImageName)).IsModified = false;
            }
            await _context.SaveChangesAsync();
            return aboutUs;
        }
    }

    public interface IAboutUsRepository
    {
        Task<AboutUs> Add(AboutUs aboutUs);
        Task<IEnumerable<AboutUs>> GetAll();
        Task<AboutUs> GetById(int id);
        Task<AboutUs> Update(AboutUs aboutUs);
        void Delete(AboutUs aboutUs);
        Task SaveChanges();
    }
}
