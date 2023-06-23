using SoftwarePal.Models;
using SoftwarePal.Repositories;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;

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
            category.CreatedAt = DateTime.Now;
            category.Slug = GenerateSlug(category.Name);
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
            if (!_categoryRepository.Exists(id))
            {
                throw new Exception("Not Found");
            }
            var category = await _categoryRepository.GetById(id);
            string origin = GetAppOrigin();
            if (category.ImageName != null)
                category.ImageName = origin + "/" + category.ImageName;
            return category;
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
                    return fullPath[fullPath.IndexOf("Images")..].Replace("\\", "/");
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

        public async Task<Category> GetBySlug(string slug)
        {
            return await _categoryRepository.GetBySlug(slug);
        }

        public string GenerateSlug(string input)
        {
            string normalizedString = input.ToLowerInvariant().Normalize(NormalizationForm.FormD).Trim();

            StringBuilder stringBuilder = new();

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
    }

    public interface ICategoryService
    {
        Task<Category> Add(Category category);
        Task<IEnumerable<Category>> GetAll();
        Task<Category> GetById(int id);
        Task<Category> Update(Category category);
        void Delete(Category category);
        string GetAppOrigin();
        Task<string> SaveImage(IFormFile image);
        Task<Category> GetBySlug(string slug);
    }
}
