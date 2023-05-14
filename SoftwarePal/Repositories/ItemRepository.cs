﻿using SoftwarePal.Data;
using SoftwarePal.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftwarePal.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly ApplicationDBContext _context;
        public ItemRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Item> Add(Item item)
        {
            await _context.Items.AddAsync(item);
            _context.SaveChanges();
            return item;
        }

        public void Delete(Item item)
        {
            _context.Items.Remove(item);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Item>> GetAll()
        {
            return await _context.Items.ToListAsync();
        }

        public async Task<Item> GetById(int id)
        {
            var subItem = await _context.Items.FindAsync(id);
            return subItem;

        }

        async Task IItemRepository.SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Item> Update(Item item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        public void SaveImage(ItemImage itemImage)
        {
            _context.ItemImages.Add(itemImage);
            _context.SaveChanges();
        }
    }

    public interface IItemRepository
    {
        Task<Item> Add(Item item);
        Task<IEnumerable<Item>> GetAll();
        Task<Item> GetById(int id);
        Task<Item> Update(Item item);
        void Delete(Item item);
        Task SaveChanges();
        void SaveImage(ItemImage itemImage);
    }
}