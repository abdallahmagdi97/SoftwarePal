using SoftwarePal.Data;
using SoftwarePal.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftwarePal.Repositories
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly ApplicationDBContext _context;
        public VoucherRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Voucher> Add(Voucher voucher)
        {
            await _context.Vouchers.AddAsync(voucher);
            _context.SaveChanges();
            return voucher;
        }

        public void Delete(Voucher voucher)
        {
            _context.Vouchers.Remove(voucher);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Voucher>> GetAll()
        {
            return await _context.Vouchers.ToListAsync();
        }

        public async Task<Voucher> GetById(int id)
        {
            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher == null)
            {
                throw new InvalidOperationException($"Voucher with ID {id} not found.");
            }
            return voucher;

        }

        async Task IVoucherRepository.SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Voucher> Update(Voucher voucher)
        {
            _context.Entry(voucher).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return voucher;
        }
        public bool Exists(int id)
        {
            return _context.Vouchers.Any(e => e.Id == id);
        }
    }

    public interface IVoucherRepository
    {
        Task<Voucher> Add(Voucher voucher);
        Task<IEnumerable<Voucher>> GetAll();
        Task<Voucher> GetById(int id);
        Task<Voucher> Update(Voucher voucher);
        void Delete(Voucher voucher);
        Task SaveChanges();
        bool Exists(int id);
    }
}
