using Hop.Framework.Domain.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using Hop.Framework.EFCore.Context;

namespace Hop.Net5WebApi.Infra.UoW
{
    public class InMemoryUoW : IUnitOfWork
    {
        private readonly HopContextBase _context;

        public InMemoryUoW(HopContextBase context)
        {
            _context = context;
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
        }

        public int Save()
        {
            _context.SaveChanges();
            return 1;
        }

        public int SaveAndCommit()
        {
            return 1;
        }

        public async Task<int> SaveAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _context.SaveChangesAsync();
            return 1;
        }

        public async Task<int> SaveAndCommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _context.SaveChangesAsync();
            return 1;
        }

        public void Commit()
        {
        }

        public void Rollback()
        {
        }

        public Task<string> SaveAndCommitAsyncWithSaveResult(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
