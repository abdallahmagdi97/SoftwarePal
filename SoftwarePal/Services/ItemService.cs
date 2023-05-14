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

        public ItemService(IItemRepository itemRepository, IIncludedSubItemRepository includedSubItemRepository, IItemPriceRuleRepository itemPriceRuleRepository, IWebHostEnvironment hostingEnvironment)
        {
            _itemRepository = itemRepository;
            _includedSubItemRepository = includedSubItemRepository;
            _itemPriceRuleRepository = itemPriceRuleRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<Item> Add(Item item)
        {
            Item savedItem = await _itemRepository.Add(item);
            ItemsHelper itemsHelper = new ItemsHelper(_includedSubItemRepository, _itemPriceRuleRepository, _itemRepository, _hostingEnvironment);
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

        public Task<Item> GetById(int id)
        {
            return _itemRepository.GetById(id);
        }

        public Task SaveChanges()
        {
            return _itemRepository.SaveChanges();
        }

        public async Task<Item> Update(Item item)
        {
            return await _itemRepository.Update(item);
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
    }
}
