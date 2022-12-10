using test.Data;
using test.Models;
using test.Repository.IRepository;

namespace test.Repository
{
    public class RepositoryOrderDetail:Repository<OrderDetail>, IRepositoryOrderDetail
    {
        private readonly ApplicationDbContext _context;
        public RepositoryOrderDetail(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        public void Update(OrderDetail entity)
        {
            _context.OrderDetails.Update(entity);
        }

    }
}
