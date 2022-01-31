using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace MyCompany.NameProject.Infrastructure.Persistence.Common.Transactions
{
    /// <summary>
    /// Расширения по работе с базой данных
    /// </summary>
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Создает или переиспользует текущую транзакцию
        /// </summary>
        /// <param name="database">База данных</param>
        /// <param name="cancellationToken">Токен асинхронной операции</param>
        /// <returns>Транзакция</returns>
        public static async Task<IDbContextTransaction> BeginOrUseTransactionAsync(
            this DatabaseFacade database, CancellationToken cancellationToken)
        {
            if (database == null)
                throw new ArgumentNullException(nameof(database));

            // WARNING: при in-memory тестировании CurrentTransaction всегда не имеет значения, так как транзакции не поддерживаются

            var currentTransaction = database.CurrentTransaction;
            return currentTransaction == null
                ? await database.BeginTransactionAsync(cancellationToken)
                : new DatabaseNopTransaction(currentTransaction.TransactionId);
        }
    }
}