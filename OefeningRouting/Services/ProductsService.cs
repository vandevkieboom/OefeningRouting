using OefeningRouting.Models;
using System.Collections.Concurrent;

namespace OefeningRouting.Services
{
    public class ProductsService : IProductsService
    {
        private readonly ConcurrentDictionary<int, Product> _products = new();
        private int _nextId = 1;

        public Task<List<Product>> GetAllProducts()
        {
            var products = _products.Values.ToList();
            return Task.FromResult(products);
        }

        public Task<Product?> GetProductById(int id)
        {
            _products.TryGetValue(id, out var product);
            return Task.FromResult(product);
        }

        public Task CreateProduct(Product product)
        {
            product.Id = _nextId++;
            _products[product.Id] = product;
            return Task.CompletedTask;
        }

        public Task<Product?> UpdateProduct(int id, Product product)
        {
            if (!_products.ContainsKey(id))
            {
                return Task.FromResult<Product?>(null);
            }

            _products[id] = product;
            return Task.FromResult(product);
        }

        public Task DeleteProduct(int id)
        {
            _products.TryRemove(id, out _);
            return Task.CompletedTask;
        }

        public Task<List<Product>> SearchProductsByName(string name)
        {
            var products = _products.Values
                .Where(p => p.Name?.Contains(name, StringComparison.OrdinalIgnoreCase) == true)
                .ToList();
            return Task.FromResult(products);
        }

        public Task<List<Product>> GetProductsByCategoryAndPrice(string categoryName, decimal minPrice, decimal maxPrice)
        {
            var products = _products.Values
                .Where(p => p.Category?.Equals(categoryName, StringComparison.OrdinalIgnoreCase) == true &&
                            p.Price >= minPrice && p.Price <= maxPrice)
                .ToList();
            return Task.FromResult(products);
        }

        public Task<Product?> ApplyDiscountToProduct(int id, int percentage)
        {
            if (_products.TryGetValue(id, out var product))
            {
                product.Price -= product.Price * (percentage / 100.0m);
                _products[id] = product;
                return Task.FromResult(product);
            }

            return Task.FromResult<Product?>(null);
        }

        public Task DeleteMultipleProducts(List<int> ids)
        {
            foreach (var id in ids)
            {
                _products.TryRemove(id, out _);
            }

            return Task.CompletedTask;
        }
    }
}
