using SoftwarePal.Models;
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
            return await _includedSubItemRepository.Add(includedSubItem);
        }

        public void Delete(IncludedSubItem includedSubItem)
        {
            _includedSubItemRepository.Delete(includedSubItem);
        }

        public Task<IEnumerable<IncludedSubItem>> GetAll()
        {
            return _includedSubItemRepository.GetAll();
        }

        public Task<IncludedSubItem> GetById(int id)
        {
            return _includedSubItemRepository.GetById(id);
        }

        public Task SaveChanges()
        {
            return _includedSubItemRepository.SaveChanges();
        }

        public async Task<IncludedSubItem> Update(IncludedSubItem includedSubItem)
        {
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
        Task SaveChanges();
    }
}
