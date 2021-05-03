using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Hop.Framework.Domain.Repository
{
    public interface IUnitOfWork
    {
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        int SaveAndCommit();
        Task<int> SaveAndCommitAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<string> SaveAndCommitAsyncWithSaveResult(CancellationToken cancellationToken = default(CancellationToken));

        int Save();
        Task<int> SaveAsync(CancellationToken cancellationToken = default(CancellationToken));

        void Commit();
        void Rollback();
    }
}
