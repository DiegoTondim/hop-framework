using Hop.Framework.Domain.Repository;
using Hop.Framework.EFCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hop.Framework.EFCore.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ILogger<UnitOfWork> Logger { get; }
        private readonly HopContextBase _context;
        private IDbContextTransaction _transaction;

        public UnitOfWork(HopContextBase context, ILogger<UnitOfWork> logger)
        {
            Logger = logger;
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
                Logger.LogError($"Failed saving changes from unity of work: {ex.Message}. Detailes: {ex}");
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
                Logger.LogError($"Failed committing changes from unity of work: {ex.Message}. Detailes: {ex}");
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
                Logger.LogError($"Failed saving changes async from unity of work: {ex.Message}. Detailes: {ex}");
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
                Logger.LogError($"Failed committing changes async from unity of work: {ex.Message}. Detailes: {ex}");
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
                Logger.LogError($"Failed committing changes async from unity of work: {ex.Message}. Detailes: {ex}");

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
                Logger.LogError($"Failed rolling back changes from unity of work: {ex.Message}. Detailes: {ex}");
            }
        }
    }
}