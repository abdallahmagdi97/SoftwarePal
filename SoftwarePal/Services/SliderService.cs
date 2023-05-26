using SoftwarePal.Models;
using SoftwarePal.Repositories;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace SoftwarePal.Services
{
    public class SliderService : ISliderService
    {
        private readonly ISliderRepository _sliderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SliderService(ISliderRepository sliderRepository, IHttpContextAccessor httpContextAccessor)
        {
            _sliderRepository = sliderRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Slider> Add(Slider slider)
        {
            slider.CreatedAt = DateTime.Now;
            return await _sliderRepository.Add(slider);
        }

        public void Delete(Slider slider)
        {
            if (!_sliderRepository.Exists(slider.Id))
                throw new InvalidOperationException($"Slider with ID {slider.Id} not found.");
            _sliderRepository.Delete(slider);
        }

        public async Task<IEnumerable<Slider>> GetAll()
        {
            return await _sliderRepository.GetAll();
        }

        public async Task<Slider> GetById(int id)
        {
            if (!_sliderRepository.Exists(id))
            {
                throw new Exception("Not Found");
            }
            var slider = await _sliderRepository.GetById(id);
            string origin = GetAppOrigin();
            if (slider.ImageName != null)
                slider.ImageName = origin + "/" + slider.ImageName;
            return slider;
        }

        public Task SaveChanges()
        {
            return _sliderRepository.SaveChanges();
        }

        public async Task<Slider> Update(Slider slider)
        {
            if (!_sliderRepository.Exists(slider.Id))
                throw new InvalidOperationException($"Slider with ID {slider.Id} not found.");
            return await _sliderRepository.Update(slider);
        }
        public async Task<string> SaveImage(IFormFile image)
        {
            string path = "";
            try
            {
                if (image.Length > 0)
                {
                    path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "Images\\Slider"));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var fullPath = Path.Combine(path, "Slider-" + Guid.NewGuid() + "." + image.ContentType.Split('/').Last());
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

    public interface ISliderService
    {
        Task<Slider> Add(Slider slider);
        Task<IEnumerable<Slider>> GetAll();
        Task<Slider> GetById(int id);
        Task<Slider> Update(Slider slider);
        void Delete(Slider slider);
        Task SaveChanges();
        Task<string> SaveImage(IFormFile image);
        string GetAppOrigin();
    }
}
