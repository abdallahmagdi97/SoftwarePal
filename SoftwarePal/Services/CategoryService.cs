using SoftwarePal.Models;
using SoftwarePal.Repositories;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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
            var savedCategory = await _categoryRepository.Add(category);
            string path = "";
            if (category.Image.Length > 0)
            {
                // C:\\Users\\aedris\\source\\repos\\SoftwarePal\\SoftwarePal
                // _hostingEnvironment?.WebRootPath
                path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "Images\\Category"));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fullPath = Path.Combine(path, "Category-" + Guid.NewGuid());
                using (var stream = File.Create(fullPath))
                {
                    category.Image.CopyTo(stream);
                }
                category.ImageName = fullPath.Substring(fullPath.IndexOf("Images")).Replace("\\", "/");
                await _categoryRepository.SaveImage(category);
            }
            savedCategory.Image = null;
            return savedCategory;
        }
        public string GetAppOrigin()
        {
            var request = _httpContextAccessor.HttpContext?.Request;

            var origin = $"{request?.Scheme}://{request?.Host}";

            return origin;
        }
        public void Delete(Category category)
        {
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
            category.ImageName = origin + "/" + category.ImageName;
            return category;
        }

        public Task SaveChanges()
        {
            return _categoryRepository.SaveChanges();
        }

        public async Task<Category> Update(Category category)
        {
            return await _categoryRepository.Update(category);
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
    }
}
