﻿using SoftwarePal.Data;
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
        public async Task<Cart> AddToCart(Cart cart)
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

        public async Task<List<Cart>> GetAll()
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

        public async Task<Cart> Update(Cart cart)
        {
            _context.Entry(cart).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<Cart> GetCartByUserId(string id)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == id);
            if (cart != null)
            {
                cart.CartItems = await _context.CartItems.Where(c => c.CartId == cart.Id).ToListAsync();
                
            }
            return cart;
        }
        public bool Exists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }

        Task ICartRepository.RemoveFromCart(Cart cart)
        {
            throw new NotImplementedException();
        }
    }

    public interface ICartRepository
    {
        Task<Cart> AddToCart(Cart cart);
        Task<List<Cart>> GetAll();
        Task<Cart> GetById(int id);
        Task<Cart> Update(Cart cart);
        Task RemoveFromCart(Cart cart);
        Task<Cart> GetCartByUserId(string id);
        bool Exists(int id);
    }
}
