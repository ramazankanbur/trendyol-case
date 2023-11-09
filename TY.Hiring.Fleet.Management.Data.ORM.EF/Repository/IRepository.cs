using System.Linq.Expressions;

namespace TY.Hiring.Fleet.Management.Data.ORM.EF.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll();
        Task<T?> GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(Expression<Func<T, bool>> predicate);
    }
}
