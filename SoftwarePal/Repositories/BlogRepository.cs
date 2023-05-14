﻿using SoftwarePal.Data;
using SoftwarePal.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftwarePal.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ApplicationDBContext _context;
        public BlogRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Blog> Add(Blog blog)
        {
            await _context.Blogs.AddAsync(blog);
            _context.SaveChanges();
            return blog;
        }

        public void Delete(Blog blog)
        {
            _context.Blogs.Remove(blog);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Blog>> GetAll()
        {
            return await _context.Blogs.ToListAsync();
        }

        public async Task<Blog> GetById(int id)
        {
            var subItem = await _context.Blogs.FindAsync(id);
            return subItem;

        }

        async Task IBlogRepository.SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Blog> Update(Blog blog)
        {
            _context.Entry(blog).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return blog;
        }
    }

    public interface IBlogRepository
    {
        Task<Blog> Add(Blog blog);
        Task<IEnumerable<Blog>> GetAll();
        Task<Blog> GetById(int id);
        Task<Blog> Update(Blog blog);
        void Delete(Blog blog);
        Task SaveChanges();
    }
}