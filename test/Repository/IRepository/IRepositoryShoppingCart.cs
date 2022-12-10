using test.Models;

namespace test.Repository.IRepository
{
    public interface IRepositoryShoppingCart:IRepository<ShoppingCart>
    {
        void Increment(ShoppingCart entity,int count);

        void Decrement(ShoppingCart entity,int count);
    }
}
