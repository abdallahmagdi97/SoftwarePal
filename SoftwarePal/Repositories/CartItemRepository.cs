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
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                throw new ArgumentNullException($"CartItem with ID {id} not found.");
            }
            return cartItem;

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

        public async Task<List<CartItem>> GetCartItemsByCartId(int id)
        {
            return await _context.CartItems.Where(i => i.CartId == id).ToListAsync();
        }
        public bool Exists(int id)
        {
            return _context.CartItems.Any(e => e.Id == id);
        }

        public async Task<CartItem> RemoveCartItem(int itemId, int qty, int cartId)
        {
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(c => c.ItemId == itemId && c.CartId == cartId);
            if (cartItem == null)
                throw new Exception("Item dosen't exist in cart!");
            if (qty == cartItem?.Qty)
            {
                Delete(cartItem);
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
        void Delete(CartItem cartItem);
        Task SaveChanges();
        Task<List<CartItem>> GetCartItemsByCartId(int id);
        bool Exists(int id);
        Task<CartItem> RemoveCartItem(int itemId, int qty, int id);
    }
}
