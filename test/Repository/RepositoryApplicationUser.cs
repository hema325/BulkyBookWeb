using test.Data;
using test.Models;
using test.Repository.IRepository;

namespace test.Repository
{
    public class RepositoryApplicationUser:Repository<ApplicationUser>, IRepositoryApplicationUser
    {
        private readonly ApplicationDbContext _context;
        public RepositoryApplicationUser(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        public void Update(ApplicationUser entity)
        {
            _context.ApplicationUsers.Update(entity);
        }
    }
}
