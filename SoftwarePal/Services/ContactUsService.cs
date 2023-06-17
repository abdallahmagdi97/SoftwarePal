using SoftwarePal.Models;
using SoftwarePal.Repositories;

namespace SoftwarePal.Services
{
    public class ContactUsService : IContactUsService
    {
        private readonly IContactUsRepository _contactUsRepository;

        public ContactUsService(IContactUsRepository contactUsRepository)
        {
            _contactUsRepository = contactUsRepository;
        }
        public async Task<List<ContactUs>> GetAllContactInquiries()
        {
            return await _contactUsRepository.GetAllContactInquiries();
        }
        public async Task AddContactInquiry(ContactUs contactUs)
        {
            await _contactUsRepository.AddContactInquiry(contactUs);
        }
    }
    public interface IContactUsService
    {
        Task<List<ContactUs>> GetAllContactInquiries();
        Task AddContactInquiry(ContactUs contactUs);
    }
}
