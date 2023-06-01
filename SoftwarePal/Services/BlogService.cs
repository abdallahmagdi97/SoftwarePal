﻿using SoftwarePal.Models;
using SoftwarePal.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftwarePal.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BlogService(IBlogRepository blogRepository, IHttpContextAccessor httpContextAccessor)
        {
            _blogRepository = blogRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Blog> Add(Blog blog)
        {
            blog.CreatedAt = DateTime.Now;
            return await _blogRepository.Add(blog);
        }

        public void Delete(Blog blog)
        {
            if (!_blogRepository.Exists(blog.Id))
            {
                throw new Exception("Not Found");
            }
            _blogRepository.Delete(blog);
        }

        public async Task<IEnumerable<Blog>> GetAll()
        {
            var blogList = await _blogRepository.GetAll();
            foreach (var blog in blogList)
            {
                string origin = GetAppOrigin();
                if (blog.ImageName != null)
                    if (!blog.ImageName.Contains("http"))
                        blog.ImageName = origin + "/" + blog.ImageName;
            }
            return blogList;
        }

        public async Task<Blog> GetById(int id)
        {
            if (!_blogRepository.Exists(id))
            {
                throw new Exception("Not Found");
            }
            var blog = await _blogRepository.GetById(id);
            string origin = GetAppOrigin();
            if (blog.ImageName != null)
                blog.ImageName = origin + "/" + blog.ImageName;
            return blog;
        }

        public async Task<Blog> Update(Blog blog)
        {
            if (!_blogRepository.Exists(blog.Id))
            {
                throw new Exception("Not Found");
            }
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
                    var fullPath = Path.Combine(path, "Blog-" + Guid.NewGuid() + "." + image.ContentType.Split('/').Last());
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
        public string GetAppOrigin()
        {
            var request = _httpContextAccessor.HttpContext?.Request;

            var origin = $"{request?.Scheme}://{request?.Host}";

            return origin;
        }

    }

    public interface IBlogService
    {
        Task<Blog> Add(Blog blog);
        Task<IEnumerable<Blog>> GetAll();
        Task<Blog> GetById(int id);
        Task<Blog> Update(Blog blog);
        void Delete(Blog blog);
        Task<string> SaveImage(IFormFile image);
        string GetAppOrigin();
    }
}
