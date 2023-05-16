using SoftwarePal.Models;
using SoftwarePal.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftwarePal.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<Blog> Add(Blog blog)
        {
            return await _blogRepository.Add(blog);
        }

        public void Delete(Blog blog)
        {
            _blogRepository.Delete(blog);
        }

        public Task<IEnumerable<Blog>> GetAll()
        {
            return _blogRepository.GetAll();
        }

        public Task<Blog> GetById(int id)
        {
            return _blogRepository.GetById(id);
        }

        public Task SaveChanges()
        {
            return _blogRepository.SaveChanges();
        }

        public async Task<Blog> Update(Blog blog)
        {
            return await _blogRepository.Update(blog);
        }

        public async Task<string> SaveImage(IFormFile image)
        {
            string path = "";
            try
            {
                if (image.Length > 0)
                {
                    path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "Images\\Blog"));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var fullPath = Path.Combine(path, "Blog-" + Guid.NewGuid());
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                    return fullPath;
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

    public interface IBlogService
    {
        Task<Blog> Add(Blog blog);
        Task<IEnumerable<Blog>> GetAll();
        Task<Blog> GetById(int id);
        Task<Blog> Update(Blog blog);
        void Delete(Blog blog);
        Task SaveChanges();
        Task<string> SaveImage(IFormFile image);
    }
}
