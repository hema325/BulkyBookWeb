using System.Linq.Expressions;

namespace test.Repository.IRepository
{
    public interface IRepository<T> where T: class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(string? includeProperties);

        void RemoveRange(IEnumerable<T> entities);

        IEnumerable<T> GetAll(Expression<Func<T,bool>> filter,string? includeProperties);

        T Get(int id);

        T GetFirstOrDefault(Expression<Func<T, bool>> filter);
        T GetFirstOrDefault(Expression<Func<T,bool>> filter,string? includeProperties);

        void Remove(T entity);
        void RemoveAll(IEnumerable<T> entities);

        void Add(T entity);



    }
}
