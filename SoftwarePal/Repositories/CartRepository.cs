using SoftwarePal.Data;
using SoftwarePal.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwarePal.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDBContext _context;
        public CartRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Cart> Add(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            _context.SaveChanges();
            return cart;
        }

        public void Delete(Cart cart)
        {
            _context.Carts.Remove(cart);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Cart>> GetAll()
        {
            return await _context.Carts.ToListAsync();
        }

        public async Task<Cart> GetById(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                throw new ArgumentNullException($"Cart with ID {id} not found.");
            }
            return cart;

        }

        async Task ICartRepository.SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Cart> Update(Cart cart)
        {
            _context.Entry(cart).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<Cart> GetCartByUserId(string id)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == id);
            if (cart == null)
            {
                throw new ArgumentNullException($"Cart with user ID {id} not found.");
            }
            return cart;
        }
    }

    public interface ICartRepository
    {
        Task<Cart> Add(Cart cart);
        Task<IEnumerable<Cart>> GetAll();
        Task<Cart> GetById(int id);
        Task<Cart> Update(Cart cart);
        void Delete(Cart cart);
        Task SaveChanges();
        Task<Cart> GetCartByUserId(string id);
    }
}
