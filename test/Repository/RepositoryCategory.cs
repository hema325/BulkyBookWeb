using test.Repository.IRepository;
using test.Data;
using test.Models;

namespace test.Repository
{
    public class RepositoryCategory : Repository<Category>, IRepositoryCategory
    {
        ApplicationDbContext db;
        public RepositoryCategory(ApplicationDbContext _db) : base(_db)
        {
            db = _db;
        }

        public void Update(Category obj)
        {
            db.Categories.Update(obj);
        }
    }
}
