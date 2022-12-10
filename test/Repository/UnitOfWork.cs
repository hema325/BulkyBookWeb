using test.Repository.IRepository;
using test.Data;

namespace test.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext db;
        public IRepositoryCategory category { get; private set; }
        public IRepositoryCoverType coverType { get; private set; }

        public IRepositoryProduct product { get; private set; }

        public IRepositoryCompany company { get; private set; }

        public IRepositoryShoppingCart shoppingCart { get; private set; }

        public IRepositoryOrderHeader orderHeader { get; private set; }

        public IRepositoryOrderDetail orderDetail { get; private set; }
        public IRepositoryApplicationUser applicationUser { get; private set; }

        public UnitOfWork(ApplicationDbContext _db)
        {
            db = _db;
            category = new RepositoryCategory(db);
            coverType = new RepositoryCoverType(db);
            product = new RepositoryProduct(db);
            company=new RepositoryCompany(db);
            shoppingCart = new RepositoryShoppingCart(db);
            orderHeader = new RepositoryOrderHeader(db);
            orderDetail=new RepositoryOrderDetail(db);
            applicationUser=new RepositoryApplicationUser(db);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
