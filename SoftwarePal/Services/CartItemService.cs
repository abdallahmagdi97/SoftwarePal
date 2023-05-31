using SoftwarePal.Models;
using SoftwarePal.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftwarePal.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;

        public CartItemService(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        public async Task<CartItem> Add(CartItem cartItem)
        {
            cartItem.CreatedAt = DateTime.Now;
            return await _cartItemRepository.Add(cartItem);
        }

        public void Delete(CartItem cartItem)
        {
            if (!_cartItemRepository.Exists(cartItem.Id))
            {
                throw new Exception("Not Found");
            }
            _cartItemRepository.Delete(cartItem);
        }

        public Task<IEnumerable<CartItem>> GetAll()
        {
            return _cartItemRepository.GetAll();
        }

        public Task<CartItem> GetById(int id)
        {
            if (!_cartItemRepository.Exists(id))
            {
                throw new Exception("Not Found");
            }
            return _cartItemRepository.GetById(id);
        }

        public Task SaveChanges()
        {
            return _cartItemRepository.SaveChanges();
        }

        public async Task<CartItem> Update(CartItem cartItem)
        {
            if (!_cartItemRepository.Exists(cartItem.Id))
            {
                throw new Exception("Not Found");
            }
            return await _cartItemRepository.Update(cartItem);
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsByCartId(int id)
        {
            return await _cartItemRepository.GetCartItemsByCartId(id);
        }
    }

    public interface ICartItemService
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
