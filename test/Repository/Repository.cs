using test.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using test.Data;

namespace test.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        ApplicationDbContext db;
        internal DbSet<T> dbset;

        public Repository(ApplicationDbContext _db)
        {
            db = _db;
            dbset = db.Set<T>();
        }

        public void Add(T entity)
        {
            dbset.Add(entity);
        }

        public T Get(int id)
        {   
            return dbset.Find(id);
        }

        public IEnumerable<T> GetAll(string? includeProperties)
        {
            IQueryable<T> query = dbset;
            if (includeProperties != null)
            {
                foreach(var pro in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query=query.Include(pro); 
                }
            }
            return query.ToList();
        }

        public IEnumerable<T> GetAll()
        {
            return dbset;
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties)
        {
            IQueryable<T> query = dbset.Where(filter);
            if (includeProperties != null)
            {
                foreach (var pro in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(pro);
                }
            }
            return query.FirstOrDefault();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbset.Where(filter);
            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbset.Remove(entity);
        }

        public void RemoveAll(IEnumerable<T> entities)
        {
            dbset.RemoveRange(entities);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter, string? includeProperties)
        {
            
            IQueryable<T> query=dbset.Where(filter);
            foreach (var pro in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(pro);
            }
            return query.ToList();
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbset.RemoveRange(entities);
        }
    }
}
