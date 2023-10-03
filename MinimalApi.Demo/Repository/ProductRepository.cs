using Microsoft.EntityFrameworkCore;
using MinimalApi.Demo.Data;
using MinimalApi.Demo.Models;
using MinimalApi.Demo.Repository.IRepository;
using static Azure.Core.HttpHeader;

namespace MinimalApi.Demo.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task CreateAsync(Product product)
        {
            _db.Add(product);
        }

        public async Task<ICollection<Product>> GetAllAsync()
        {
            return await _db.Products.ToListAsync();
        }

        public async Task<Product> GetAsync(int id)
        {
            return await _db.Products.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Product> GetAsync(string productName)
        {
            return await _db.Products.FirstOrDefaultAsync(u => u.Name.ToLower() == productName.ToLower());
        }

        public async Task RemoveAsync(Product product)
        {
            _db.Products.Remove(product);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _db.Products.Update(product);
        }
    }
}
