using SoftwarePal.Data;
using SoftwarePal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace SoftwarePal.Repositories
{
    public class SliderRepository : ISliderRepository
    {
        private readonly ApplicationDBContext _context;
        public SliderRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Slider> Add(Slider slider)
        {
            await _context.Sliders.AddAsync(slider);
            _context.SaveChanges();
            return slider;
        }

        public void Delete(Slider slider)
        {
            _context.Sliders.Remove(slider);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Slider>> GetAll()
        {
            return await _context.Sliders.OrderBy(c => c.Order).ToListAsync();
        }

        public async Task<Slider> GetById(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null)
            {
                throw new InvalidOperationException($"Slider with ID {id} not found.");
            }
            return slider;

        }

        public async Task<Slider> Update(Slider slider)
        {
            _context.Entry(slider).State = EntityState.Modified;
            if (slider.ImageName == null)
            {
                _context.Entry(slider).Property(nameof(slider.ImageName)).IsModified = false;
            }
            await _context.SaveChangesAsync();
            return slider;
        }
        public bool Exists(int id)
        {
            return _context.Sliders.Any(e => e.Id == id);
        }
    }

    public interface ISliderRepository
    {
        Task<Slider> Add(Slider slider);
        Task<IEnumerable<Slider>> GetAll();
        Task<Slider> GetById(int id);
        Task<Slider> Update(Slider slider);
        void Delete(Slider slider);
        bool Exists(int id);
    }

}
