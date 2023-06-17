using Microsoft.EntityFrameworkCore;
using SoftwarePal.Data;
using SoftwarePal.Models;

namespace SoftwarePal.Repositories
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public StatisticsRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Statistics> GetProductStatistics(int productId)
        {
            return _dbContext.Statistics
                .Where(s => s.ProductId == productId)
                .ToList();
        }

        public void UpdateProductStatistics(Statistics statistics)
        {
            _dbContext.Statistics.Update(statistics);
            _dbContext.SaveChanges();
        }
    }

    public interface IStatisticsRepository
    {
        List<Statistics> GetProductStatistics(int productId);
        void UpdateProductStatistics(Statistics statistics);

    }
}
