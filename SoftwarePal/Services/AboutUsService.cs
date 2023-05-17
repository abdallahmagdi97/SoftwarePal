﻿using Microsoft.AspNetCore.Mvc;
using SoftwarePal.Models;
using SoftwarePal.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftwarePal.Services
{
    public class AboutUsService : IAboutUsService
    {
        private readonly IAboutUsRepository _aboutUsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AboutUsService(IAboutUsRepository aboutUsRepository, IHttpContextAccessor httpContextAccessor)
        {
            _aboutUsRepository = aboutUsRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AboutUs> Add(AboutUs aboutUs)
        {
            return await _aboutUsRepository.Add(aboutUs);
        }

        public void Delete(AboutUs aboutUs)
        {
            _aboutUsRepository.Delete(aboutUs);
        }

        public Task<IEnumerable<AboutUs>> GetAll()
        {
            return _aboutUsRepository.GetAll();
        }

        public Task<AboutUs> GetById(int id)
        {
            return _aboutUsRepository.GetById(id);
        }

        public Task SaveChanges()
        {
            return _aboutUsRepository.SaveChanges();
        }

        public async Task<AboutUs> Update(AboutUs aboutUs)
        {
            return await _aboutUsRepository.Update(aboutUs);
        }

        public async Task<string> SaveImage(IFormFile image)
        {
            string path = "";
            try
            {
                if (image.Length > 0)
                {
                    path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "Images\\AboutUs"));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var fullPath = Path.Combine(path, "AboutUs-" + Guid.NewGuid() + "." + image.ContentType.Split('/').Last());
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                    var appOrigin = GetAppOrigin();
                    return appOrigin + "/" + fullPath.Substring(fullPath.IndexOf("Images")).Replace("\\", "/");
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
            var request = _httpContextAccessor.HttpContext.Request;

            var origin = $"{request.Scheme}://{request.Host}";

            return origin;
        }
        public async Task<byte[]> GetImage(string imageName)
        {

            Byte[] b;
            b = await File.ReadAllBytesAsync(Path.Combine(Environment.CurrentDirectory, "Images\\AboutUs", $"{imageName}"));
            return b;
        }
    }

    public interface IAboutUsService
    {
        Task<AboutUs> Add(AboutUs aboutUs);
        Task<IEnumerable<AboutUs>> GetAll();
        Task<AboutUs> GetById(int id);
        Task<AboutUs> Update(AboutUs aboutUs);
        void Delete(AboutUs aboutUs);
        Task SaveChanges();
        Task<string> SaveImage(IFormFile image);
        Task<byte[]> GetImage(string imageName);
        string GetAppOrigin();
    }
}
