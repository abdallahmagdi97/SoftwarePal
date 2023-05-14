﻿using SoftwarePal.Models;
using SoftwarePal.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace SoftwarePal.Helpers
{
    public class ItemsHelper
    {
        private readonly IIncludedSubItemRepository _includedSubItemRepository;
        private readonly IItemPriceRuleRepository _itemPriceRuleRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ItemsHelper(IIncludedSubItemRepository includedSubItemRepository, IItemPriceRuleRepository itemPriceRuleRepository, IItemRepository itemRepository, IWebHostEnvironment hostingEnvironment)
        {
            _includedSubItemRepository = includedSubItemRepository;
            _itemPriceRuleRepository = itemPriceRuleRepository;
            _itemRepository = itemRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        internal async void AddIncludedSubItems(Item item, int id)
        {
            foreach(var includedItem in item.IncludedSubItems)
            {
                includedItem.ItemId = id;
                await _includedSubItemRepository.Add(includedItem);
            }
        }

        internal async void AddItemPriceRules(Item item, int id)
        {
            foreach (var priceRule in item.ItemPriceRules)
            {
                priceRule.ItemId = id;
                await _itemPriceRuleRepository.Add(priceRule);
            }
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
                    var filePath = Path.Combine("G:\\SP", "Images", "Item", item.ItemImages[i].ImageName);
                    item.ItemImages[i].ImageName = filePath;
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