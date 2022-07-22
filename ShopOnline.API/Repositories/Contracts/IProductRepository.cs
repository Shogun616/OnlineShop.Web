using ShopOnline.API.Entities;

namespace ShopOnline.API.Repositories.Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetItems();
        Task<IEnumerable<ProductCategory>> GetCategories();
        Task<Product> GetItem(int Id);
        Task<ProductCategory> GetCategory(int Id);
    }
}
