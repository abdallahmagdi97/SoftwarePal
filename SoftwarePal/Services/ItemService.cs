using SoftwarePal.Helpers;
using SoftwarePal.Models;
using SoftwarePal.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftwarePal.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IIncludedSubItemRepository _includedSubItemRepository;
        private readonly IItemPriceRuleRepository _itemPriceRuleRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ItemService(IItemRepository itemRepository, IIncludedSubItemRepository includedSubItemRepository, 
            IItemPriceRuleRepository itemPriceRuleRepository, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _itemRepository = itemRepository;
            _includedSubItemRepository = includedSubItemRepository;
            _itemPriceRuleRepository = itemPriceRuleRepository;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Item> Add(Item item)
        {
            Item savedItem = await _itemRepository.Add(item);
            ItemsHelper itemsHelper = new ItemsHelper(_includedSubItemRepository, _itemPriceRuleRepository, _itemRepository, _hostingEnvironment, _httpContextAccessor);
            itemsHelper.SaveImages(item, savedItem.Id);
            itemsHelper.AddItemPriceRules(item, savedItem.Id);
            itemsHelper.AddIncludedSubItems(item, savedItem.Id);
            return savedItem;
        }

        public void Delete(Item item)
        {
            _itemRepository.Delete(item);
        }

        public Task<IEnumerable<Item>> GetAll()
        {
            return _itemRepository.GetAll();
        }

        public async Task<Item> GetById(int id)
        {
            var item = await _itemRepository.GetById(id);
            string origin = GetAppOrigin();
            item = await GetItemImages(origin, item);
            return item;
        }

        public Task SaveChanges()
        {
            return _itemRepository.SaveChanges();
        }

        public async Task<Item> Update(Item item)
        {
            return await _itemRepository.Update(item);
        }
        public string GetAppOrigin()
        {
            var request = _httpContextAccessor.HttpContext?.Request;

            var origin = $"{request?.Scheme}://{request?.Host}";

            return origin;
        }

        public async Task<Item> GetItemImages(string origin, Item item)
        {
            item.ItemImages = await _itemRepository.GetItemImages(item.Id);
            for(int i = 0; i < item.ItemImages.Count; i++)
            {
                item.ItemImages[i].ImageName = origin + "/" + item.ItemImages[i].ImageName;
            }
            return item;
        }
    }

    public interface IItemService
    {
        Task<Item> Add(Item item);
        Task<IEnumerable<Item>> GetAll();
        Task<Item> GetById(int id);
        Task<Item> Update(Item item);
        void Delete(Item item);
        Task SaveChanges();
        string GetAppOrigin();
        Task<Item> GetItemImages(string origin, Item item);
        Task<decimal> GetPricefromPriceRole();
    }
}
