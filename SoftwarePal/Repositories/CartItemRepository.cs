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

        public async Task Delete(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CartItem>> GetAll()
        {
            return await _context.CartItems.ToListAsync();
        }

        public async Task<CartItem> GetById(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                throw new ArgumentNullException($"CartItem with ID {id} not found.");
            }
            return cartItem;

        }

        public async Task<CartItem> Update(CartItem cartItem)
        {
            _context.Entry(cartItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<List<CartItem>> GetCartItemsByCartId(int id)
        {
            return await _context.CartItems.Where(i => i.CartId == id).ToListAsync();
        }
        public async Task<bool> Exists(int id)
        {
            return await _context.CartItems.AnyAsync(e => e.Id == id);
        }

        public async Task<CartItem> RemoveCartItem(int itemId, int qty, int cartId)
        {
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(c => c.ItemId == itemId && c.CartId == cartId);
            if (cartItem == null)
                throw new Exception("Item dosen't exist in cart!");
            if (qty == cartItem?.Qty)
            {
                await Delete(cartItem);
            } else
            {
                if (qty > cartItem?.Qty)
                    throw new ArgumentException("Item quantity you are trying to remove is more than the items in your cart!");
                cartItem.Qty -= qty;
                await Update(cartItem);
            }
            return cartItem;
        }
    }

    public interface ICartItemRepository
    {
        Task<CartItem> Add(CartItem cartItem);
        Task<IEnumerable<CartItem>> GetAll();
        Task<CartItem> GetById(int id);
        Task<CartItem> Update(CartItem cartItem);
        Task Delete(CartItem cartItem);
        Task<List<CartItem>> GetCartItemsByCartId(int id);
        Task<bool> Exists(int id);
        Task<CartItem> RemoveCartItem(int itemId, int qty, int id);
    }
}
