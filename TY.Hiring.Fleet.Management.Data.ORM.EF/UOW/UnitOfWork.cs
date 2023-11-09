using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TY.Hiring.Fleet.Management.Data.ORM.EF.Repository;

namespace TY.Hiring.Fleet.Management.Data.ORM.EF.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private bool _disposed;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public IRepository<T> GetRepository<T>() where T : BaseEntity
        {
            return new Repository<T>(_context);
        }

        public async Task<IDbContextTransaction> BeginNewTransaction()
        {
            IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();

            return transaction;
        }

        public async Task<bool> RollBackTransaction(IDbContextTransaction transaction)
        {
            try
            {
                await transaction.RollbackAsync();
                transaction = null;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task TransactionCommit(IDbContextTransaction transaction)
        {
            if (transaction != null)
            {
                try
                {
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error on save changes ", ex);
                }
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                if (_context == null)
                {
                    throw new ArgumentException("Context is null");
                }

                int result = await _context.SaveChangesAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error on save changes ", ex);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
