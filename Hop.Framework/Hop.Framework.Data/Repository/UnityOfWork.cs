using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hop.Framework.Core.Log;
using Hop.Framework.EFCore.Context;
using Hop.Framework.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hop.Framework.EFCore.Repository
{
    public class UnityOfWork : IUnityOfWork
    {
        public ILogProvider LogProvider { get; }
        private readonly HopContextBase _context;
        private IDbContextTransaction _transaction;

        public UnityOfWork(HopContextBase context, ILogProvider logProvider)
        {
            LogProvider = logProvider;
            _context = context;
            _transaction = null;
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _transaction = _context.Database.BeginTransaction(isolationLevel);
        }

        public int Save()
        {
            try
            {
                var result = _context.ChangeTracker.HasChanges() ? _context.SaveChanges() : 0;

                return result;
            }
            catch (Exception ex)
            {
                LogProvider.Error($"Failed saving changes from unity of work: {ex.Message}. Detailes: {ex}");
                return 0;
            }
        }

        public int SaveAndCommit()
        {
            try
            {
                var result = this.Save();
                this.Commit();
                return result;
            }
            catch (Exception ex)
            {
                LogProvider.Error($"Failed committing changes from unity of work: {ex.Message}. Detailes: {ex}");
                return 0;
            }
        }

        public async Task<int> SaveAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                return await (_context.ChangeTracker.HasChanges() ? _context.SaveChangesAsync(cancellationToken) : Task.FromResult(0));
            }
            catch (Exception ex)
            {
                LogProvider.Error($"Failed saving changes async from unity of work: {ex.Message}. Detailes: {ex}");
                return await Task.FromResult(0);
            }
        }

        public async Task<int> SaveAndCommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await this.SaveAsync(cancellationToken);
                this.Commit();
                return result;
            }
            catch (Exception ex)
            {
                LogProvider.Error($"Failed committing changes async from unity of work: {ex.Message}. Detailes: {ex}");
                return await Task.FromResult(0);
            }
        }

        public async Task<string> SaveAndCommitAsyncWithSaveResult(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await this.SaveAsync(cancellationToken);
                this.Commit();
                return result.ToString();
            }
            catch (Exception ex)
            {
                LogProvider.Error($"Failed committing changes async from unity of work: {ex.Message}. Detailes: {ex}");

                return await Task.FromResult(ex.Message);
            }
        }

        public void Commit()
        {
            _transaction?.Commit();
            _transaction?.Dispose();
            _transaction = null;
        }

        public void Rollback()
        {
            try
            {
                var changedEntries = _context.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();
                foreach (var entry in changedEntries)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entry.CurrentValues.SetValues(entry.OriginalValues);
                            entry.State = EntityState.Unchanged;
                            break;
                        case EntityState.Added:
                            entry.State = EntityState.Detached;
                            break;
                        case EntityState.Deleted:
                            entry.State = EntityState.Unchanged;
                            break;
                    }
                }

                if (_transaction != null)
                {
                    _transaction.Rollback();
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
            catch (Exception ex)
            {
                LogProvider.Error($"Failed rolling back changes from unity of work: {ex.Message}. Detailes: {ex}");
            }
        }
    }
}