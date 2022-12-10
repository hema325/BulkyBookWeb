using test.Models;

namespace test.Repository.IRepository
{
    public interface IRepositoryCategory:IRepository<Category>
    {
        public void Update(Category obj);
    }
}
