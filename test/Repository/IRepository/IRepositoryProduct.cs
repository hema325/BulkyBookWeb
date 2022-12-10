using test.Models;

namespace test.Repository.IRepository
{
    public interface IRepositoryProduct:IRepository<Product>
    {
        void Update(Product entity);
    }
}
