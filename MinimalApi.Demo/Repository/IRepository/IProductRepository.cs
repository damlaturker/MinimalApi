using MinimalApi.Demo.Models;
using static Azure.Core.HttpHeader;

namespace MinimalApi.Demo.Repository.IRepository
{
    public interface IProductRepository
    {
        Task<ICollection<Product>> GetAllAsync();
        Task<Product> GetAsync(int id);
        Task<Product> GetAsync(string productName);
        Task CreateAsync(Product product);
        Task UpdateAsync(Product product);
        Task RemoveAsync(Product product);
        Task SaveAsync();
    }
}
