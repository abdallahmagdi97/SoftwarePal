using SoftwarePal.Data;
using SoftwarePal.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftwarePal.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly ApplicationDBContext _context;
        public ItemRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Item> Add(Item item)
        {
            await _context.Items.AddAsync(item);
            _context.SaveChanges();
            return item;
        }

        public void Delete(Item item)
        {
            _context.Items.Remove(item);
            _context.SaveChanges();
        }

        public async Task<List<Item>> GetAll()
        {
            return await _context.Items.ToListAsync();
        }

        public async Task<Item> GetById(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                throw new InvalidOperationException($"Item with ID {id} not found.");
            }
            return item;

        }

        public async Task<Item> Update(Item item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        public void SaveImage(ItemImage itemImage)
        {
            _context.ItemImages.Add(itemImage);
            _context.SaveChanges();
        }

        public async Task<List<ItemImage>> GetItemImages(int itemId)
        {
            return await _context.ItemImages.Where(x => x.ItemId == itemId).OrderBy(c => c.ImageOrder).ToListAsync();
        }
        public async Task<bool> Exists(int id)
        {
            return await _context.Items.AnyAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Item>> GetItemsByCategory(int categoryId)
        {
            return await _context.Items.Where(x => x.CategoryId == categoryId).ToListAsync();
        }
        public async Task<List<ItemPriceRule>> GetItemPriceRules(int itemId)
        {
            return await _context.ItemPriceRules.Where(x => x.ItemId == itemId).OrderBy(p => p.Price).ToListAsync();
        }

        public async Task<List<SubItem>> GetIncludedSubItems(int itemId)
        {
            var includedSubItems = await _context.IncludedSubItems.Where(x => x.ItemId == itemId).Select(i => i.SubItemId).ToListAsync();
            var subItems = await _context.SubItems.Where(s => includedSubItems.Contains(s.Id)).ToListAsync();
            return subItems;
        }

        public async Task<Item> GetBySlug(string slug)
        {
            return await _context.Items.FirstOrDefaultAsync(i => i.Slug == slug);
        }

        public async Task<List<Item>> GetFeaturedItems()
        {
            return await _context.Items.Where(i => i.IsFeatured == true).ToListAsync();
        }
    }

    public interface IItemRepository
    {
        Task<Item> Add(Item item);
        Task<List<Item>> GetAll();
        Task<Item> GetById(int id);
        Task<Item> Update(Item item);
        void Delete(Item item);
        void SaveImage(ItemImage itemImage);
        Task<List<ItemImage>> GetItemImages(int itemId);
        Task<List<ItemPriceRule>> GetItemPriceRules(int itemId);
        Task<List<SubItem>> GetIncludedSubItems(int itemId);
        Task<bool> Exists(int id);
        Task<IEnumerable<Item>> GetItemsByCategory(int categoryId);
        Task<Item> GetBySlug(string slug);
        Task<List<Item>> GetFeaturedItems();
    }
}
