using SoftwarePal.Data;
using SoftwarePal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            return await _context.Sliders.ToListAsync();
        }

        public async Task<Slider> GetById(int id)
        {
            var subItem = await _context.Sliders.FindAsync(id);
            return subItem;

        }

        async Task ISliderRepository.SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Slider> Update(Slider slider)
        {
            _context.Entry(slider).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return slider;
        }
    }

    public interface ISliderRepository
    {
        Task<Slider> Add(Slider slider);
        Task<IEnumerable<Slider>> GetAll();
        Task<Slider> GetById(int id);
        Task<Slider> Update(Slider slider);
        void Delete(Slider slider);
        Task SaveChanges();
    }

}
