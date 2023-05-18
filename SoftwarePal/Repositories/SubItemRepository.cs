using SoftwarePal.Data;
using SoftwarePal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftwarePal.Repositories
{
    public class SubItemRepository : ISubItemRepository
    {
        private readonly ApplicationDBContext _context;
        public SubItemRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<SubItem> Add(SubItem subItem)
        {
            await _context.SubItems.AddAsync(subItem);
            _context.SaveChanges();
            return subItem;
        }

        public void Delete(SubItem subItem)
        {
            _context.SubItems.Remove(subItem);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<SubItem>> GetAll()
        {
            return await _context.SubItems.ToListAsync();
        }

        public async Task<SubItem> GetById(int id)
        {
            var subItem = await _context.SubItems.FindAsync(id);
            if (subItem == null)
            {
                throw new InvalidOperationException($"SubItem with ID {id} not found.");
            }
            return subItem;

        }

        async Task ISubItemRepository.SaveChanges()
        {
             await _context.SaveChangesAsync();
        }

        public async Task<SubItem> Update(SubItem subItem)
        {
            _context.Entry(subItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return subItem;
        }
        public async void SaveImage(SubItem subItem)
        {
            await Update(subItem);
        }
    }

    public interface ISubItemRepository
    {
        Task<SubItem> Add(SubItem subItem);
        Task<IEnumerable<SubItem>> GetAll();
        Task<SubItem> GetById(int id);
        Task<SubItem> Update(SubItem subItem);
        void Delete(SubItem subItem);
        Task SaveChanges();
        void SaveImage(SubItem subItem);
    }
}
