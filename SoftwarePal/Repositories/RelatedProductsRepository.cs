using Microsoft.EntityFrameworkCore;
using SoftwarePal.Data;
using SoftwarePal.Models;

namespace SoftwarePal.Repositories
{
    public class RelatedProductsRepository : IRelatedProductsRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public RelatedProductsRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<RelatedProducts> GetRelatedProducts(int productId)
        {
            return _dbContext.RelatedProducts
                .Where(rp => rp.ProductId == productId)
                .Include(rp => rp.RelatedProductId)
                .ToList();
        }

        public void AddRelatedProduct(int productId, int relatedProductId)
        {
            var relatedProducts = new RelatedProducts
            {
                ProductId = productId,
                RelatedProductId = relatedProductId
            };

            _dbContext.RelatedProducts.Add(relatedProducts);
            _dbContext.SaveChanges();
        }

        public void RemoveRelatedProduct(int productId, int relatedProductId)
        {
            var relatedProducts = _dbContext.RelatedProducts
                .FirstOrDefault(rp => rp.ProductId == productId && rp.RelatedProductId == relatedProductId);

            if (relatedProducts != null)
            {
                _dbContext.RelatedProducts.Remove(relatedProducts);
                _dbContext.SaveChanges();
            }
        }
    }

    public interface IRelatedProductsRepository
    {
        List<RelatedProducts> GetRelatedProducts(int productId);
        void AddRelatedProduct(int productId, int relatedProductId);
        void RemoveRelatedProduct(int productId, int relatedProductId);
    }
}
