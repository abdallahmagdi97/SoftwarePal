using SoftwarePal.Models;
using SoftwarePal.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftwarePal.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IItemService _itemService;

        public CartService(ICartRepository cartRepository, ICartItemRepository cartItemRepository, IItemService itemService)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _itemService = itemService;
        }

        public async Task<Cart> AddToCart(int itemId, int Qty, string userId)
        {
            var cart = await GetCartByUserId(userId);
            if (await _itemService.Exists(itemId))
            {
                if (cart == null)
                {


                    cart = new Cart() { UserId = userId, CreatedAt = DateTime.Now };
                    await _cartRepository.AddToCart(cart);
                    var cartItem = new CartItem() { ItemId = itemId, Qty = Qty, CreatedAt = DateTime.Now, CartId = cart.Id, UserCreated = userId };
                    await _cartItemRepository.Add(cartItem);
                    cart.CartItems = new List<CartItem>();
                    cart.CartItems.Add(cartItem);

                }
                else
                {
                    cart.UpdatedAt = DateTime.Now;
                    var cartItem = new CartItem() { ItemId = itemId, Qty = Qty, CreatedAt = DateTime.Now, CartId = cart.Id, UserCreated = userId };
                    await _cartItemRepository.Add(cartItem);
                    cart.CartItems = await _cartItemRepository.GetCartItemsByCartId(cart.Id);
                }
            }else
            {
                throw new ArgumentException($"Item with Id {itemId} dosen't exist!");
            }
            return cart;
        }

        public async Task RemoveFromCart(int itemId, int Qty, string userId)
        {
            var cart = await GetCartByUserId(userId);
            if (cart == null)
                throw new ArgumentException("User has no cart to remove items from");
            cart.CartItems = await _cartItemRepository.GetCartItemsByCartId(cart.Id);
            await _cartItemRepository.RemoveCartItem(itemId, Qty, cart.Id);
        }

        public async Task<List<Cart>> GetAll()
        {
            return await _cartRepository.GetAll();
        }

        public async Task<Cart> GetById(int id)
        {
            if (!_cartRepository.Exists(id))
            {
                throw new Exception("Not Found");
            }
            return await _cartRepository.GetById(id);
        }

        public async Task<Cart> Update(Cart cart)
        {
            if (!_cartRepository.Exists(cart.Id))
            {
                throw new Exception("Not Found");
            }
            return await _cartRepository.Update(cart);
        }

        public async Task<Cart> GetCartByUserId(string id)
        {
            var cart = await _cartRepository.GetCartByUserId(id);
            if (cart.CartItems != null)
            {
                foreach (var item in cart.CartItems)
                {
                    item.ItemDetails = await _itemService.GetById(item.ItemId);
                }
            }
            return cart;
        }
    }

    public interface ICartService
    {
        Task<Cart> AddToCart(int itemId, int Qty, string userId);
        Task<List<Cart>> GetAll();
        Task<Cart> GetById(int id);
        Task<Cart> Update(Cart cart);
        Task RemoveFromCart(int itemId, int Qty, string userId);
        Task<Cart> GetCartByUserId(string id);
    }
}
