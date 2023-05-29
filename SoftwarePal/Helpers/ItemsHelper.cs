using SoftwarePal.Models;
using SoftwarePal.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

namespace SoftwarePal.Helpers
{
    public class ItemsHelper
    {
        private readonly IIncludedSubItemRepository _includedSubItemRepository;
        private readonly IItemPriceRuleRepository _itemPriceRuleRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ItemsHelper(IIncludedSubItemRepository includedSubItemRepository, IItemPriceRuleRepository itemPriceRuleRepository, IItemRepository itemRepository, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _includedSubItemRepository = includedSubItemRepository;
            _itemPriceRuleRepository = itemPriceRuleRepository;
            _itemRepository = itemRepository;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }
        internal async Task AddIncludedSubItems(Item item, int id)
        {
            foreach(var includedItem in item.IncludedSubItems)
            {
                includedItem.ItemId = id;
                includedItem.CreatedAt = DateTime.Now;
                includedItem.UserCreated = item.UserCreated;
                await _includedSubItemRepository.Add(includedItem);
            }
        }
        internal async Task AddItemPriceRules(Item item, int id)
        {
            foreach (var priceRule in item.ItemPriceRules)
            {
                priceRule.ItemId = id;
                priceRule.CreatedAt = DateTime.Now;
                priceRule.UserCreated = item.UserCreated;
                await _itemPriceRuleRepository.Add(priceRule);
            }
        }
        internal async Task UpdateIncludedSubItems(Item item, int id)
        {
            foreach (var includedItem in item.IncludedSubItems)
            {
                if (includedItem.Id == 0)
                {
                    await AddIncludedSubItems(item, id);
                }
                else
                {
                    includedItem.ItemId = id;
                    includedItem.UpdatedAt = DateTime.Now;
                    includedItem.UserUpdated = item.UserUpdated;
                    await _includedSubItemRepository.Update(includedItem);
                }
            }
        }
        internal async Task UpdateItemPriceRules(Item item, int id)
        {
            foreach (var priceRule in item.ItemPriceRules)
            {
                if (priceRule.Id == 0)
                {
                    await AddItemPriceRules(item, id);
                }
                else
                {
                    priceRule.ItemId = id;
                    priceRule.UpdatedAt = DateTime.Now;
                    priceRule.UserUpdated = item.UserUpdated;
                    await _itemPriceRuleRepository.Update(priceRule);
                }
            }
        }
        public string GetAppOrigin()
        {
            var request = _httpContextAccessor.HttpContext?.Request;

            var origin = $"{request?.Scheme}://{request?.Host}";

            return origin;
        }
        internal void SaveImages(Item item, int id)
        {
            long size = item.ItemImages.Count;
            List<string> images = new List<string>();
            for (int i = 0; i < item.ItemImages.Count; i++)
            {
                if (item.ItemImages[i].Image.Length > 0)
                {
                    // C:\\Users\\aedris\\source\\repos\\SoftwarePal\\SoftwarePal
                    var filePath = Path.Combine(Environment.CurrentDirectory, "Images", "Item", (item.ItemImages[i].ImageName ?? "item-") + Guid.NewGuid() + "." + item.ItemImages[i].Image.ContentType.Split('/').Last());
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    item.ItemImages[i].ImageName = filePath.Substring(filePath.IndexOf("Images")).Replace("\\", "/");
                    images.Add(filePath);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        item.ItemImages[i].Image.CopyTo(stream);
                    }
                }
                item.ItemImages[i].ItemId = id;
                _itemRepository.SaveImage(item.ItemImages[i]);
            }
        }
    }
}
