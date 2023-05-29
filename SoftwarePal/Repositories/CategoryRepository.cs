using SoftwarePal.Data;
using SoftwarePal.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace SoftwarePal.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDBContext _context;
        public CategoryRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Category> Add(Category category)
        {
            await _context.Categories.AddAsync(category);
            _context.SaveChanges();
            return category;
        }

        public void Delete(Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _context.Categories.OrderBy(c => c.Order).ToListAsync();
        }

        public async Task<Category> GetById(int id)
        {
            var Category = await _context.Categories.FindAsync(id);
            if (Category == null)
            {
                throw new ArgumentNullException($"Category with ID {id} not found.");
            }
            return Category;

        }

        async Task ICategoryRepository.SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Category> Update(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            if (category.ImageName == null)
            {
                _context.Entry(category).Property(nameof(category.ImageName)).IsModified = false;
            }
            await _context.SaveChangesAsync();
            return category;
        }
        public bool Exists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }

        public async Task<Category> GetBySlug(string slug)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Slug == slug);
        }
    }

    public interface ICategoryRepository
    {
        Task<Category> Add(Category category);
        Task<IEnumerable<Category>> GetAll();
        Task<Category> GetById(int id);
        Task<Category> Update(Category category);
        void Delete(Category category);
        Task SaveChanges();
        bool Exists(int id);
        Task<Category> GetBySlug(string slug);
    }
}
