using test.Data;
using test.Models;
using test.Repository.IRepository;

namespace test.Repository
{
    public class RepositoryCompany:Repository<Company>,IRepositoryCompany
    {
        private readonly ApplicationDbContext _context;

        public RepositoryCompany(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Company entity)
        {
            _context.Companies.Update(entity);
        }
    }
}
