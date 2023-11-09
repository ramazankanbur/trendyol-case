using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace TY.Hiring.Fleet.Management.Data.ORM.EF.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {

        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = dbContext.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            var removeItems = _dbSet.Where(predicate).ToList();
            _dbSet.RemoveRange(removeItems);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        } 
       
        public async Task<T?> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
