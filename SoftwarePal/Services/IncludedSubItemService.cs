﻿using SoftwarePal.Models;
using SoftwarePal.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftwarePal.Services
{
    public class IncludedSubItemService : IIncludedSubItemService
    {
        private readonly IIncludedSubItemRepository _includedSubItemRepository;

        public IncludedSubItemService(IIncludedSubItemRepository includedSubItemRepository)
        {
            _includedSubItemRepository = includedSubItemRepository;
        }

        public async Task<IncludedSubItem> Add(IncludedSubItem includedSubItem)
        {
            includedSubItem.CreatedAt = DateTime.Now;
            return await _includedSubItemRepository.Add(includedSubItem);
        }

        public void Delete(IncludedSubItem includedSubItem)
        {
            if (!_includedSubItemRepository.Exists(includedSubItem.Id))
            {
                throw new Exception("Not Found");
            }
            _includedSubItemRepository.Delete(includedSubItem);
        }

        public async Task<IEnumerable<IncludedSubItem>> GetAll()
        {
            return await _includedSubItemRepository.GetAll();
        }

        public Task<IncludedSubItem> GetById(int id)
        {
            if (!_includedSubItemRepository.Exists(id))
            {
                throw new Exception("Not Found");
            }
            return _includedSubItemRepository.GetById(id);
        }

        public async Task<IncludedSubItem> Update(IncludedSubItem includedSubItem)
        {
            if (_includedSubItemRepository.Exists(includedSubItem.Id))
            {
                throw new Exception("Not Found");
            }
            return await _includedSubItemRepository.Update(includedSubItem);
        }
    }

    public interface IIncludedSubItemService
    {
        Task<IncludedSubItem> Add(IncludedSubItem includedSubItem);
        Task<IEnumerable<IncludedSubItem>> GetAll();
        Task<IncludedSubItem> GetById(int id);
        Task<IncludedSubItem> Update(IncludedSubItem includedSubItem);
        void Delete(IncludedSubItem includedSubItem);
    }
}
