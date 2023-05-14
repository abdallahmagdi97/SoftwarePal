using SoftwarePal.Models;
using SoftwarePal.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftwarePal.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<Cart> Add(Cart cart)
        {
            return await _cartRepository.Add(cart);
        }

        public void Delete(Cart cart)
        {
            _cartRepository.Delete(cart);
        }

        public Task<IEnumerable<Cart>> GetAll()
        {
            return _cartRepository.GetAll();
        }

        public Task<Cart> GetById(int id)
        {
            return _cartRepository.GetById(id);
        }

        public Task SaveChanges()
        {
            return _cartRepository.SaveChanges();
        }

        public async Task<Cart> Update(Cart cart)
        {
            return await _cartRepository.Update(cart);
        }

        public async Task<Cart> GetCartByUserId(string id)
        {
            return await _cartRepository.GetCartByUserId(id);
        }
    }

    public interface ICartService
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
