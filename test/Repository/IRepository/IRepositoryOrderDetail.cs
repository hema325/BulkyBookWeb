using test.Models;

namespace test.Repository.IRepository
{
    public interface IRepositoryOrderDetail:IRepository<OrderDetail>
    {
        void Update(OrderDetail entity);
    }
}
