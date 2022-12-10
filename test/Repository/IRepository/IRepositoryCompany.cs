using test.Models;

namespace test.Repository.IRepository
{
    public interface IRepositoryCompany :IRepository<Company>
    {
        void Update(Company entity);
    }
}
