using SoftwarePal.Models;
using SoftwarePal.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftwarePal.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _voucherRepository;

        public VoucherService(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }

        public async Task<Voucher> Add(Voucher voucher)
        {
            return await _voucherRepository.Add(voucher);
        }

        public void Delete(Voucher voucher)
        {
            _voucherRepository.Delete(voucher);
        }

        public Task<IEnumerable<Voucher>> GetAll()
        {
            return _voucherRepository.GetAll();
        }

        public Task<Voucher> GetById(int id)
        {
            return _voucherRepository.GetById(id);
        }

        public Task SaveChanges()
        {
            return _voucherRepository.SaveChanges();
        }

        public async Task<Voucher> Update(Voucher voucher)
        {
            return await _voucherRepository.Update(voucher);
        }
    }

    public interface IVoucherService
    {
        Task<Voucher> Add(Voucher voucher);
        Task<IEnumerable<Voucher>> GetAll();
        Task<Voucher> GetById(int id);
        Task<Voucher> Update(Voucher voucher);
        void Delete(Voucher voucher);
        Task SaveChanges();
    }
}
