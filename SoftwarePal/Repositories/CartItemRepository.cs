using SoftwarePal.Data;
using SoftwarePal.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftwarePal.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly ApplicationDBContext _context;
        public CartItemRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<CartItem> Add(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
            _context.SaveChanges();
            return cartItem;
        }

        public void Delete(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<CartItem>> GetAll()
        {
            return await _context.CartItems.ToListAsync();
        }

        public async Task<CartItem> GetById(int id)
        {
            var subItem = await _context.CartItems.FindAsync(id);
            return subItem;

        }

        async Task ICartItemRepository.SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<CartItem> Update(CartItem cartItem)
        {
            _context.Entry(cartItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsByCartId(int id)
        {
            return await _context.CartItems.Where(i => i.CartId == id).ToListAsync();
        }
    }

    public interface ICartItemRepository
    {
        Task<CartItem> Add(CartItem cartItem);
        Task<IEnumerable<CartItem>> GetAll();
        Task<CartItem> GetById(int id);
        Task<CartItem> Update(CartItem cartItem);
        void Delete(CartItem cartItem);
        Task SaveChanges();
        Task<IEnumerable<CartItem>> GetCartItemsByCartId(int id);
    }
}
