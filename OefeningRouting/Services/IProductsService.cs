using OefeningRouting.Models;

namespace OefeningRouting.Services
{
    public interface IProductsService
    {
        public Task<List<Product>> GetAllProducts();
        public Task<Product?> GetProductById(int id);
        public Task CreateProduct(Product product);
        public Task<Product?> UpdateProduct(int id, Product product);
        public Task DeleteProduct(int id);
        public Task<List<Product>> SearchProductsByName(string name);
        public Task<List<Product>> GetProductsByCategoryAndPrice(string categoryName, decimal minPrice, decimal maxPrice);
        public Task<Product?> ApplyDiscountToProduct(int id, int percentage);
        public Task DeleteMultipleProducts(List<int> ids);
    }
}
