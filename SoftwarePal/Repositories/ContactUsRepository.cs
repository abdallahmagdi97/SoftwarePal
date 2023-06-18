using Microsoft.EntityFrameworkCore;
using SoftwarePal.Data;
using SoftwarePal.Models;

namespace SoftwarePal.Repositories
{
    public class ContactUsRepository : IContactUsRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public ContactUsRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<ContactUs>> GetAllContactInquiries()
        {
            return await _dbContext.ContactUs.ToListAsync();
        }
        public async Task AddContactInquiry(ContactUs contactUs)
        {
            await _dbContext.ContactUs.AddAsync(contactUs);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ContactUs> GetContactInquiryById(int contactInquiryId)
        {
            return await _dbContext.ContactUs.FindAsync(contactInquiryId);
        }

        public async Task MarkAsRead(ContactUs contactInquiry)
        {
            contactInquiry.IsRead = true;
            _dbContext.Update(contactInquiry);
            await _dbContext.SaveChangesAsync();
        }
    }
    public interface IContactUsRepository
    {
        Task<List<ContactUs>> GetAllContactInquiries();
        Task AddContactInquiry(ContactUs contactUs);
        Task<ContactUs> GetContactInquiryById(int contactInquiryId);
        Task MarkAsRead(ContactUs contactInquiry);
    }
}
