using SoftwarePal.Models;
using SoftwarePal.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftwarePal.Services
{
    public class WishListService : IWishListService
    {
        private readonly IWishListRepository _wishListRepository;
        private readonly IItemRepository _itemRepository;

        public WishListService(IWishListRepository wishListRepository, IItemRepository itemRepository)
        {
            _wishListRepository = wishListRepository;
            _itemRepository = itemRepository;
        }

        public async Task<WishList> Add(string userId, int itemId)
        {
            if (!await _itemRepository.Exists(itemId))
                throw new ArgumentNullException("Item dosen't exist!");
            var wishList = new WishList() { ItemId = itemId, UserId = userId, CreatedAt = DateTime.Now };
            return await _wishListRepository.Add(wishList);
        }

        public async Task Delete(string userId, int itemId)
        {
            await _wishListRepository.Delete(userId, itemId);
        }

        public async Task<List<WishList>> GetAll(string userId)
        {
            return await _wishListRepository.GetAll(userId);
        }

        public async Task<WishList> GetById(int id)
        {
            if (!_wishListRepository.Exists(id))
            {
                throw new Exception("Not Found");
            }
            var wishList = await _wishListRepository.GetById(id);
            return wishList;
        }

        public Task SaveChanges()
        {
            return _wishListRepository.SaveChanges();
        }

        public async Task<WishList> Update(WishList wishList)
        {
            if (!_wishListRepository.Exists(wishList.Id))
            {
                throw new Exception("Not Found");
            }
            return await _wishListRepository.Update(wishList);
        }

    }

    public interface IWishListService
    {
        Task<WishList> Add(string userId, int itemId);
        Task<List<WishList>> GetAll(string userId);
        Task<WishList> GetById(int id);
        Task<WishList> Update(WishList wishList);
        Task Delete(string userId, int itemId);
        Task SaveChanges();
    }
}
