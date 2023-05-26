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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SubItemService(ISubItemRepository subItemRepository, IHttpContextAccessor httpContextAccessor)
        {
            _subItemRepository = subItemRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<SubItem> Add(SubItem subItem)
        {
            subItem.CreatedAt = DateTime.Now;
            await _subItemRepository.Add(subItem);
            string path;
            if (subItem.Image.Length > 0)
            {
                path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "Images\\SubItem"));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fullPath = Path.Combine(path, "SubItem-" + Guid.NewGuid() + "." + subItem.Image.ContentType.Split('/').Last());
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await subItem.Image.CopyToAsync(fileStream);
                }
                subItem.ImageName = fullPath.Substring(fullPath.IndexOf("Images")).Replace("\\", "/");
                _subItemRepository.SaveImage(subItem);
            }

            return subItem;
        }

        public void Delete(SubItem subItem)
        {
            if (!_subItemRepository.Exists(subItem.Id))
                throw new InvalidOperationException($"SubItem with ID {subItem.Id} not found.");
            _subItemRepository.Delete(subItem);
        }

        public async Task<IEnumerable<SubItem>> GetAll()
        {
            return await _subItemRepository.GetAll();
        }

        public async Task<SubItem> GetById(int id)
        {
            var subItem = await _subItemRepository.GetById(id);
            string origin = GetAppOrigin();
            subItem.ImageName = origin + "/" + subItem.ImageName;
            return subItem;
        }

        public Task SaveChanges()
        {
            return _subItemRepository.SaveChanges();
        }

        public async Task<SubItem> Update(SubItem subItem)
        {
            if (!_subItemRepository.Exists(subItem.Id))
                throw new InvalidOperationException($"SubItem with ID {subItem.Id} not found.");
            return await _subItemRepository.Update(subItem);
        }
        public string GetAppOrigin()
        {
            var request = _httpContextAccessor.HttpContext?.Request;

            var origin = $"{request?.Scheme}://{request?.Host}";

            return origin;
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
            string GetAppOrigin();

        }

}
