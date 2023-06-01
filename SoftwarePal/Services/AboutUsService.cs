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
            aboutUs.CreatedAt = DateTime.Now;
            return await _aboutUsRepository.Add(aboutUs);
        }

        public void Delete(AboutUs aboutUs)
        {
            if (!_aboutUsRepository.Exists(aboutUs.Id))
            {
                throw new Exception("Not Found");
            }
            _aboutUsRepository.Delete(aboutUs);
        }

        public async Task<IEnumerable<AboutUs>> GetAll()
        {
            var aboutUsList = await _aboutUsRepository.GetAll();
            foreach(var aboutUs in aboutUsList)
            {
                string origin = GetAppOrigin();
                if (aboutUs.ImageName != null)
                    if (!aboutUs.ImageName.Contains("http"))
                        aboutUs.ImageName = origin + "/" + aboutUs.ImageName;
            }
            return aboutUsList;
        }

        public async Task<AboutUs> GetById(int id)
        {
            if (!_aboutUsRepository.Exists(id))
            {
                throw new Exception("Not Found");
            }
            var aboutUs = await _aboutUsRepository.GetById(id);
            string origin = GetAppOrigin();
            if (aboutUs.ImageName != null)
                aboutUs.ImageName = origin + "/" + aboutUs.ImageName;
            return aboutUs;
        }

        public async Task<AboutUs> Update(AboutUs aboutUs)
        {
            if (!_aboutUsRepository.Exists(aboutUs.Id))
            {
                throw new Exception("Not Found");
            }
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
        Task<string> SaveImage(IFormFile image);
        Task<byte[]> GetImage(string imageName);
        string GetAppOrigin();
    }
}
