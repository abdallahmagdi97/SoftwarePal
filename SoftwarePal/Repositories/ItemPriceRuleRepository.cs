using SoftwarePal.Data;
using SoftwarePal.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftwarePal.Repositories
{
    public class ItemPriceRuleRepository : IItemPriceRuleRepository
    {
        private readonly ApplicationDBContext _context;
        public ItemPriceRuleRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<ItemPriceRule> Add(ItemPriceRule item)
        {
            await _context.ItemPriceRules.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public void Delete(ItemPriceRule item)
        {
            _context.ItemPriceRules.Remove(item);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<ItemPriceRule>> GetAll()
        {
            return await _context.ItemPriceRules.ToListAsync();
        }

        public async Task<ItemPriceRule> GetById(int id)
        {
            var itemPriceRule = await _context.ItemPriceRules.FindAsync(id);
            if (itemPriceRule == null)
            {
                throw new InvalidOperationException($"ItemPriceRule with ID {id} not found.");
            }
            return itemPriceRule;

        }
        public async Task<ItemPriceRule> Update(ItemPriceRule item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }
        public bool Exists(int id)
        {
            return _context.ItemPriceRules.Any(e => e.Id == id);
        }
    }

    public interface IItemPriceRuleRepository
    {
        Task<ItemPriceRule> Add(ItemPriceRule item);
        Task<IEnumerable<ItemPriceRule>> GetAll();
        Task<ItemPriceRule> GetById(int id);
        Task<ItemPriceRule> Update(ItemPriceRule item);
        void Delete(ItemPriceRule item);
        bool Exists(int id);
    }
}
