using SoftwarePal.Models;
using SoftwarePal.Repositories;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata;

namespace SoftwarePal.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryService(ICategoryRepository categoryRepository, IHttpContextAccessor httpContextAccessor)
        {
            _categoryRepository = categoryRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Category> Add(Category category)
        {
            return await _categoryRepository.Add(category);
            
        }
        public string GetAppOrigin()
        {
            var request = _httpContextAccessor.HttpContext?.Request;

            var origin = $"{request?.Scheme}://{request?.Host}";

            return origin;
        }
        public void Delete(Category category)
        {
            if (!_categoryRepository.Exists(category.Id))
            {
                throw new Exception("Not Found");
            }

            _categoryRepository.Delete(category);
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            var categoryList = await _categoryRepository.GetAll();
            foreach (var category in categoryList)
            {
                string origin = GetAppOrigin();
                category.ImageName = origin + "/" + category.ImageName;
            }
            return categoryList;
        }

        public async Task<Category> GetById(int id)
        {
            var category = await _categoryRepository.GetById(id);
            string origin = GetAppOrigin();
            if (category.ImageName != null)
                category.ImageName = origin + "/" + category.ImageName;
            return category;
        }

        public Task SaveChanges()
        {
            return _categoryRepository.SaveChanges();
        }

        public async Task<Category> Update(Category category)
        {
            if (!_categoryRepository.Exists(category.Id))
            {
                throw new Exception("Not Found");
            }
            return await _categoryRepository.Update(category);
        }
        public async Task<string> SaveImage(IFormFile image)
        {
            string path = "";
            try
            {
                if (image.Length > 0)
                {
                    path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "Images\\Category"));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var fullPath = Path.Combine(path, "Category-" + Guid.NewGuid() + "." + image.ContentType.Split('/').Last());
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                    return fullPath.Substring(fullPath.IndexOf("Images")).Replace("\\", "/");
                }
                else
                {
                    throw new Exception("File empty");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("File Copy Failed", ex);
            }
        }
    }

    public interface ICategoryService
    {
        Task<Category> Add(Category category);
        Task<IEnumerable<Category>> GetAll();
        Task<Category> GetById(int id);
        Task<Category> Update(Category category);
        void Delete(Category category);
        Task SaveChanges();
        string GetAppOrigin();
        Task<string> SaveImage(IFormFile image);
    }
}
