using test.Data;
using test.Models;
using test.Repository.IRepository;

namespace test.Repository
{
    public class RepositoryShoppingCart:Repository<ShoppingCart>,IRepositoryShoppingCart
    {
        private readonly ApplicationDbContext _context;
        public RepositoryShoppingCart(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Decrement(ShoppingCart entity, int count)
        {
            entity.count -= count;
        }

        public void Increment(ShoppingCart entity, int count)
        {
            entity.count += count;
        }
    }
}
