using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Interfaces;
using ProductManager.Infrastructure.Contexts;


namespace ProductManager.Infrastructure.Repositories
{
    public class Repository : IRepository<Product>
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public Repository(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<Product> Add(Product item)
        {
            _context.Products.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task Edit(Product item)
        {
            var existingEntity = _context.Products.Local.FirstOrDefault(e => e.Id == item.Id);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = EntityState.Detached;
            }
            _context.Products.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> Get(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAll(int pageNumber, int pageSize)
        {
            var cacheKey = $"ProductList-{pageNumber}-{pageSize}";
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Product> productList))
            {
                productList = await _context.Products
                    .Where(x => !x.Deleted)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(60));

                _cache.Set(cacheKey, productList, cacheEntryOptions);
            }
            return productList;
        }

        public async Task<IEnumerable<Product>> GetByName(string namePart)
        {
            return await _context.Products
                .Where(p => p.Name.ToUpper().Contains(namePart.ToUpper()))
                .ToListAsync();
        }

        public async Task SoftDelete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.Deleted = true;
                await _context.SaveChangesAsync();
            }
        }
    }

}
