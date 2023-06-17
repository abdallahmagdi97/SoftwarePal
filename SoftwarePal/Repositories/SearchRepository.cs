using Microsoft.EntityFrameworkCore;
using SoftwarePal.Data;
using SoftwarePal.Models;

namespace SoftwarePal.Repositories
{
    public class SearchRepository : ISearchRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public SearchRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Search> GetSearchHistory()
        {
            return _dbContext.Search.ToList();
        }

        public void AddSearchQuery(Search search)
        {
            _dbContext.Search.Add(search);
            _dbContext.SaveChanges();
        }
    }
    public interface ISearchRepository
    {
        List<Search> GetSearchHistory();
        void AddSearchQuery(Search search);
    }
}
