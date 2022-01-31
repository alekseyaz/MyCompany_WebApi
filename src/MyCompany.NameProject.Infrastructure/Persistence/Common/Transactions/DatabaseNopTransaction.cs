using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace MyCompany.NameProject.Infrastructure.Persistence.Common.Transactions
{
    /// <summary>
    /// Обертка в виде транзакции, не делающая ничего
    /// </summary>
    public class DatabaseNopTransaction : IDbContextTransaction
    {
        public Guid TransactionId { get; }

        public DatabaseNopTransaction(Guid transactionId)
        {
            TransactionId = transactionId;
        }

        public void Dispose()
        {

        }

        public void Commit()
        {

        }

        public void Rollback()
        {
            throw new NotSupportedException("Rollback in nested transaction is restricted");
        }

        public async ValueTask DisposeAsync()
        {
            Dispose();
        }

        public async Task CommitAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            Commit();
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            Rollback();
        }
    }
}
