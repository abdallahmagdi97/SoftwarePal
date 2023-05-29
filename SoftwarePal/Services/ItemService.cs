using SoftwarePal.Helpers;
using SoftwarePal.Models;
using SoftwarePal.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;

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
            item.CreatedAt = DateTime.Now;
            if (item.Name != null)
                item.Slug = GenerateSlug(item.Name);
            await _itemRepository.Add(item);
            ItemsHelper itemsHelper = new ItemsHelper(_includedSubItemRepository, _itemPriceRuleRepository, _itemRepository, _hostingEnvironment, _httpContextAccessor);
            if (item.ItemImages != null)
                itemsHelper.SaveImages(item, item.Id);
            if (item.ItemPriceRules != null)
                await itemsHelper.AddItemPriceRules(item, item.Id);
            if (item.IncludedSubItems != null)
                await itemsHelper.AddIncludedSubItems(item, item.Id);
            
            return item;
        }

        public void Delete(Item item)
        {
            _itemRepository.Delete(item);
        }

        public async Task<List<Item>> GetAll()
        {
            var items = await _itemRepository.GetAll();
            string origin = GetAppOrigin();
            for(int i = 0; i < items.Count(); i++)
            {
                items[i].ItemImages = await GetItemImages(origin, items[i]);
                items[i].ItemPriceRules = await _itemRepository.GetItemPriceRules(items[i].Id);
                items[i].IncludedSubItems = await _itemRepository.GetIncludedSubItems(items[i].Id);
            }
            return items;
        }

        public async Task<Item> GetById(int id)
        {
            if (!_itemRepository.Exists(id))
            {
                throw new Exception("Not Found");
            }
            var item = await _itemRepository.GetById(id);
            string origin = GetAppOrigin();
            item.ItemImages  = await GetItemImages(origin, item);
            item.ItemPriceRules = await _itemRepository.GetItemPriceRules(id);
            item.IncludedSubItems = await _itemRepository.GetIncludedSubItems(id);
            return item;
        }

        public Task SaveChanges()
        {
            return _itemRepository.SaveChanges();
        }

        public async Task<Item> Update(Item item)
        {
            if (!_itemRepository.Exists(item.Id))
            {
                throw new InvalidOperationException($"Item with ID {item.Id} not found.");
            }
            await _itemRepository.Update(item);
            ItemsHelper itemsHelper = new ItemsHelper(_includedSubItemRepository, _itemPriceRuleRepository, _itemRepository, _hostingEnvironment, _httpContextAccessor);
            await itemsHelper.UpdateItemPriceRules(item, item.Id);
            await itemsHelper.UpdateIncludedSubItems(item, item.Id);
            await _itemRepository.SaveChanges();
            return item;
        }
        public string GetAppOrigin()
        {
            var request = _httpContextAccessor.HttpContext?.Request;

            var origin = $"{request?.Scheme}://{request?.Host}";

            return origin;
        }

        public async Task<List<ItemImage>> GetItemImages(string origin, Item item)
        {
            item.ItemImages = await _itemRepository.GetItemImages(item.Id);
            for(int i = 0; i < item.ItemImages.Count; i++)
            {
                if (item.ItemImages[i].ImageName != null)
                    item.ItemImages[i].ImageName = origin + "/" + item.ItemImages[i].ImageName;
            }
            return item.ItemImages;
        }

        public Task<decimal> GetPricefromPriceRole()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Item>> GetItemsByCategory(int categoryId)
        {
            var items = await _itemRepository.GetItemsByCategory(categoryId);
            foreach(var item in items)
            {
                string origin = GetAppOrigin();
                item.ItemImages = await GetItemImages(origin, item);
                item.ItemPriceRules = await _itemRepository.GetItemPriceRules(item.Id);
                item.IncludedSubItems = await _itemRepository.GetIncludedSubItems(item.Id);
            }
            return items;
        }

        public string GenerateSlug(string input)
        {
            string normalizedString = input.ToLowerInvariant().Normalize(NormalizationForm.FormD);

            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark &&
                    (c == '-' || c == '_' || char.IsLetterOrDigit(c)))
                {
                    stringBuilder.Append(c);
                }
                else if (c == ' ')
                {
                    stringBuilder.Append('-');
                }
            }

            return Regex.Replace(stringBuilder.ToString(), @"-{2,}", "-"); // Remove consecutive hyphens
        }

        public async Task<Item> GetBySlug(string slug)
        {
            var item = await _itemRepository.GetBySlug(slug);
            string origin = GetAppOrigin();
            item.ItemImages = await GetItemImages(origin, item);
            item.ItemPriceRules = await _itemRepository.GetItemPriceRules(item.Id);
            item.IncludedSubItems = await _itemRepository.GetIncludedSubItems(item.Id);
            return item;
        }

        public async Task<List<Item>> GetFeaturedItems()
        {
            var items = await _itemRepository.GetFeaturedItems();
            string origin = GetAppOrigin();
            for (int i = 0; i < items.Count(); i++)
            {
                items[i].ItemImages = await GetItemImages(origin, items[i]);
                items[i].ItemPriceRules = await _itemRepository.GetItemPriceRules(items[i].Id);
                items[i].IncludedSubItems = await _itemRepository.GetIncludedSubItems(items[i].Id);
            }
            return items;
        }
    }

    public interface IItemService
    {
        Task<Item> Add(Item item);
        Task<List<Item>> GetAll();
        Task<Item> GetById(int id);
        Task<Item> GetBySlug(string slug);
        Task<Item> Update(Item item);
        void Delete(Item item);
        Task SaveChanges();
        string GetAppOrigin();
        Task<List<ItemImage>> GetItemImages(string origin, Item item);
        Task<decimal> GetPricefromPriceRole();
        Task<IEnumerable<Item>> GetItemsByCategory(int categoryId);
        string GenerateSlug(string input);
        Task<List<Item>> GetFeaturedItems();
    }
}
