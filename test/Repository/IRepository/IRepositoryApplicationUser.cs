using test.Models;

namespace test.Repository.IRepository
{
    public interface IRepositoryApplicationUser:IRepository<ApplicationUser>
    {
        void Update(ApplicationUser entity);
    }
}
