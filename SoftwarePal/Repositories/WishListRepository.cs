using SoftwarePal.Data;
using SoftwarePal.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftwarePal.Repositories
{
    public class WishListRepository : IWishListRepository
    {
        private readonly ApplicationDBContext _context;
        public WishListRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<WishList> Add(WishList wishList)
        {
            await _context.WishLists.AddAsync(wishList);
            _context.SaveChanges();
            return wishList;
        }

        public async Task Delete(string userId, int itemId)
        {
            WishList wishList = await _context.WishLists.FirstOrDefaultAsync(w => w.UserId == userId && w.ItemId == itemId);
            if (wishList == null)
            {
                throw new InvalidOperationException($"WishList not found.");
            }
            _context.WishLists.Remove(wishList);
            _context.SaveChanges();
        }

        public async Task<List<WishList>> GetAll(string userId)
        {
            return await _context.WishLists.Where(w => w.UserId == userId).ToListAsync();
        }

        public async Task<WishList> GetById(int id)
        {
            var wishList = await _context.WishLists.FindAsync(id);
            if (wishList == null)
            {
                throw new InvalidOperationException($"WishList with ID {id} not found.");
            }
            return wishList;

        }

        async Task IWishListRepository.SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<WishList> Update(WishList wishList)
        {
            _context.Entry(wishList).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return wishList;
        }
        public bool Exists(int id)
        {
            return _context.WishLists.Any(e => e.Id == id);
        }

    }

    public interface IWishListRepository
    {
        Task<WishList> Add(WishList wishList);
        Task<List<WishList>> GetAll(string userId);
        Task<WishList> GetById(int id);
        Task<WishList> Update(WishList wishList);
        Task Delete(string userId, int itemId);
        Task SaveChanges();
        bool Exists(int id);
    }
}
