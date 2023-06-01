using SoftwarePal.Data;
using SoftwarePal.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftwarePal.Repositories
{
    public class IncludedSubItemRepository : IIncludedSubItemRepository
    {
        private readonly ApplicationDBContext _context;
        public IncludedSubItemRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<IncludedSubItem> Add(IncludedSubItem includedSubItem)
        {
            await _context.IncludedSubItems.AddAsync(includedSubItem);
            await _context.SaveChangesAsync();
            return includedSubItem;
        }

        public void Delete(IncludedSubItem includedSubItem)
        {
            _context.IncludedSubItems.Remove(includedSubItem);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<IncludedSubItem>> GetAll()
        {
            return await _context.IncludedSubItems.ToListAsync();
        }

        public async Task<IncludedSubItem> GetById(int id)
        {
            var subItem = await _context.IncludedSubItems.FindAsync(id);
            if (subItem == null)
            {
                throw new ArgumentNullException($"SubItem with ID {id} not found.");
            }
            return subItem;

        }

        public async Task<IncludedSubItem> Update(IncludedSubItem includedSubItem)
        {
            _context.Entry(includedSubItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return includedSubItem;
        }
        public bool Exists(int id)
        {
            return _context.IncludedSubItems.Any(e => e.Id == id);
        }

        public async Task AddItemSubItem(ItemSubItems itemSubItems)
        {
            await _context.ItemSubItems.AddAsync(itemSubItems);
        }
    }

    public interface IIncludedSubItemRepository
    {
        Task<IncludedSubItem> Add(IncludedSubItem includedSubItem);
        Task<IEnumerable<IncludedSubItem>> GetAll();
        Task<IncludedSubItem> GetById(int id);
        Task<IncludedSubItem> Update(IncludedSubItem includedSubItem);
        void Delete(IncludedSubItem includedSubItem);
        bool Exists(int id);
        Task AddItemSubItem(ItemSubItems itemSubItems);
    }
}
