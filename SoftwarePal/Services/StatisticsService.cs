using SoftwarePal.Models;
using SoftwarePal.Repositories;

namespace SoftwarePal.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IStatisticsRepository _statisticsRepository;

        public StatisticsService(IStatisticsRepository statisticsRepository)
        {
            _statisticsRepository = statisticsRepository;
        }

        public List<Statistics> GetProductStatistics(int productId)
        {
            return _statisticsRepository.GetProductStatistics(productId);
        }

        public void UpdateProductStatistics(Statistics statistics)
        {
            _statisticsRepository.UpdateProductStatistics(statistics);
        }
    }

    public interface IStatisticsService
    {
        List<Statistics> GetProductStatistics(int productId);
        void UpdateProductStatistics(Statistics statistics);
    }
}
