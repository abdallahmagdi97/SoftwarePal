﻿using SoftwarePal.Models;
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

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> Add(Category category)
        {
            var savedCategory = await _categoryRepository.Add(category);

            if (category.Image.Length > 0)
            {
                // C:\\Users\\aedris\\source\\repos\\SoftwarePal\\SoftwarePal
                // _hostingEnvironment?.WebRootPath
                var filePath = Path.Combine("C:\\Users\\aedris\\source\\repos\\SoftwarePal\\SoftwarePal", "Images","Category", category.ImageName);
                category.ImageName = filePath;
                using (var stream = System.IO.File.Create(filePath))
                {
                    category.Image.CopyTo(stream);
                }
                await _categoryRepository.SaveImage(category);
            }
            savedCategory.Image = null;
            return savedCategory;
        }

        public void Delete(Category category)
        {
            _categoryRepository.Delete(category);
        }

        public Task<IEnumerable<Category>> GetAll()
        {
            return _categoryRepository.GetAll();
        }

        public Task<Category> GetById(int id)
        {
            return _categoryRepository.GetById(id);
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
    }
}