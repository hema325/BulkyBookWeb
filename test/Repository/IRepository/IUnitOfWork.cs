namespace test.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IRepositoryCategory category { get;}
        IRepositoryCoverType coverType { get; }
        IRepositoryCompany company { get; }
        IRepositoryProduct product { get; }
        IRepositoryShoppingCart shoppingCart { get; }
        IRepositoryOrderHeader orderHeader { get; }
        IRepositoryOrderDetail orderDetail { get; }
        IRepositoryApplicationUser applicationUser { get; }

        public void Save();
        
    }
}
