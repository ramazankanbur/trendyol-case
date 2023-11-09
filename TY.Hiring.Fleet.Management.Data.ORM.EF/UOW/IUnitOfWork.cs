using Microsoft.EntityFrameworkCore.Storage;
using TY.Hiring.Fleet.Management.Data.ORM.EF.Repository;

namespace TY.Hiring.Fleet.Management.Data.ORM.EF.UOW
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : BaseEntity;
        Task<IDbContextTransaction> BeginNewTransaction();
        Task<bool> RollBackTransaction(IDbContextTransaction transaction);
        Task TransactionCommit(IDbContextTransaction transaction);
        Task<int> SaveChangesAsync();
        void Dispose();
    }
}
