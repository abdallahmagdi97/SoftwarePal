using SoftwarePal.Models;
using SoftwarePal.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftwarePal.Services
{
    public class SliderService : ISliderService
    {
        private readonly ISliderRepository _sliderRepository;

        public SliderService(ISliderRepository sliderRepository)
        {
            _sliderRepository = sliderRepository;
        }

        public async Task<Slider> Add(Slider slider)
        {
            return await _sliderRepository.Add(slider);
        }

        public void Delete(Slider slider)
        {
            _sliderRepository.Delete(slider);
        }

        public Task<IEnumerable<Slider>> GetAll()
        {
            return _sliderRepository.GetAll();
        }

        public Task<Slider> GetById(int id)
        {
            return _sliderRepository.GetById(id);
        }

        public Task SaveChanges()
        {
            return _sliderRepository.SaveChanges();
        }

        public async Task<Slider> Update(Slider slider)
        {
            return await _sliderRepository.Update(slider);
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
    }
}
