using test.Data;
using test.Models;
using test.Repository.IRepository;

namespace test.Repository
{
    public class RepositoryProduct:Repository<Product>,IRepositoryProduct
    {
        private ApplicationDbContext _context;
        public RepositoryProduct(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Product entity)
        {
            if (entity.UrlImage == null)
            {
                var obj = _context.Products.FirstOrDefault(p => p.Id == entity.Id);
                entity.UrlImage = obj.UrlImage;
            }
            _context.Products.Update(entity);
        }
    }
}
