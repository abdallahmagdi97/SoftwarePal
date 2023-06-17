using SoftwarePal.Models;
using SoftwarePal.Repositories;

namespace SoftwarePal.Services
{
    public class RelatedProductsService : IRelatedProductsService
    {
        private readonly IRelatedProductsRepository _relatedProductsRepository;

        public RelatedProductsService(IRelatedProductsRepository relatedProductsRepository)
        {
            _relatedProductsRepository = relatedProductsRepository;
        }

        public List<RelatedProducts> GetRelatedProducts(int productId)
        {
            return _relatedProductsRepository.GetRelatedProducts(productId);
        }

        public void AddRelatedProduct(int productId, int relatedProductId)
        {
            _relatedProductsRepository.AddRelatedProduct(productId, relatedProductId);
        }

        public void RemoveRelatedProduct(int productId, int relatedProductId)
        {
            _relatedProductsRepository.RemoveRelatedProduct(productId, relatedProductId);
        }
    }

    public interface IRelatedProductsService
    {
        List<RelatedProducts> GetRelatedProducts(int productId);
        void AddRelatedProduct(int productId, int relatedProductId);
        void RemoveRelatedProduct(int productId, int relatedProductId);
    }
}
