using test.Models;
using test.Repository.IRepository;
using test.Data;

namespace test.Repository
{
    public class RepositoryCoverType : Repository<CoverType>, IRepositoryCoverType
    {
        ApplicationDbContext _context;
        public RepositoryCoverType(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(CoverType obj)
        {
            _context.CoverTypes.Update(obj);
        }
    }
}
