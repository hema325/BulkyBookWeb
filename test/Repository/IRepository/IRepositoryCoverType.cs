using test.Models;

namespace test.Repository.IRepository
{
    public interface IRepositoryCoverType:IRepository<CoverType>
    {
        void Update(CoverType obj);
    }
}
