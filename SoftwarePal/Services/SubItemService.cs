using SoftwarePal.Models;
using SoftwarePal.Repositories;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SoftwarePal.Services
{
    public class SubItemService : ISubItemService
    {
        private readonly ISubItemRepository _subItemRepository;
        public SubItemService(ISubItemRepository subItemRepository)
        {
            _subItemRepository = subItemRepository;
        }

        public async Task<SubItem> Add(SubItem subItem)
        {
            var savedSubItem = await _subItemRepository.Add(subItem);

            if (subItem.Image.Length > 0)
            {
                // C:\\Users\\aedris\\source\\repos\\SoftwarePal\\SoftwarePal
                var filePath = Path.Combine("G:\\SP", "Images", "SubItem", subItem.ImageName);
                subItem.ImageName = filePath;
                using (var stream = File.Create(filePath))
                {
                    subItem.Image.CopyTo(stream);
                }
                _subItemRepository.SaveImage(subItem);
            }
            
            return savedSubItem;
        }

        public void Delete(SubItem subItem)
        {
            _subItemRepository.Delete(subItem);
        }

        public Task<IEnumerable<SubItem>> GetAll()
        {
            return _subItemRepository.GetAll();
        }

        public Task<SubItem> GetById(int id)
        {
            return _subItemRepository.GetById(id);
        }

        public Task SaveChanges()
        {
            return _subItemRepository.SaveChanges();
        }

        public async Task<SubItem> Update(SubItem subItem)
        {
            return await _subItemRepository.Update(subItem);
        }
    }

    public interface ISubItemService
    {
        Task<SubItem> Add(SubItem subItem);
        Task<IEnumerable<SubItem>> GetAll();
        Task<SubItem> GetById(int id);
        Task<SubItem> Update(SubItem subItem);
        void Delete(SubItem subItem);
        Task SaveChanges();
    }
}
