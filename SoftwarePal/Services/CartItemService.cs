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

        public async Task Delete(CartItem cartItem)
        {
            if (!await _cartItemRepository.Exists(cartItem.Id))
            {
                throw new Exception("Not Found");
            }
            await _cartItemRepository.Delete(cartItem);
        }

        public async Task<IEnumerable<CartItem>> GetAll()
        {
            return await _cartItemRepository.GetAll();
        }

        public async Task<CartItem> GetById(int id)
        {
            if (!await _cartItemRepository.Exists(id))
            {
                throw new Exception("Not Found");
            }
            return await _cartItemRepository.GetById(id);
        }

        public async Task<CartItem> Update(CartItem cartItem)
        {
            if (!await _cartItemRepository.Exists(cartItem.Id))
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
        Task Delete(CartItem cartItem);
        Task<IEnumerable<CartItem>> GetCartItemsByCartId(int id);
    }
}
