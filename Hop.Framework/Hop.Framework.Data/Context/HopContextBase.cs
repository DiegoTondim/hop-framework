﻿using Hop.Framework.Core.Date;
using Hop.Framework.Core.User;
using Hop.Framework.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Hop.Framework.EFCore.Context
{
	public class HopContextBase : DbContext
	{
		protected readonly IUserContextService ContextService;
		protected readonly IDateProvider DateProvider;
		public Guid Id { get; set; }

		public HopContextBase(IUserContextService contextService,
								 IDateProvider dateProvider) : base()
		{
			ContextService = contextService;
			DateProvider = dateProvider;
			ContextService = contextService;
			Id = Guid.NewGuid();
		}

		public HopContextBase(IUserContextService contextService,
			IDateProvider dateProvider,
			DbContextOptions options) : base(options)
		{
			DateProvider = dateProvider;
			ContextService = contextService;
			Id = Guid.NewGuid();
		}

		private const string ANONYMOUS_USER = "[Anonymous]";

		public void AutoLoadMappings<TContext>(ModelBuilder modelBuilder) where TContext : HopContextBase
		{
			var implementedConfigTypes = typeof(TContext).Assembly
				.GetTypes()
				.Where(t => !t.IsAbstract
					&& !t.IsGenericTypeDefinition
					&& t.GetTypeInfo().ImplementedInterfaces.Any(i =>
						i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)));

			foreach (var configType in implementedConfigTypes)
			{
				dynamic config = Activator.CreateInstance(configType);
				modelBuilder.ApplyConfiguration(config);
			}
		}

		public override int SaveChanges()
		{
			TrackRecord(ChangeTracker);

			CustomChangeTrackingBeforeSave(ChangeTracker);
			var result = base.SaveChanges();
			CustomChangeTrackingAfterSave(ChangeTracker);
			return result;
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			TrackRecord(ChangeTracker);

			CustomChangeTrackingBeforeSave(ChangeTracker);
			var result = base.SaveChangesAsync(cancellationToken);
			CustomChangeTrackingAfterSave(ChangeTracker);
			return result;
		}

		public virtual void CustomChangeTrackingBeforeSave(ChangeTracker changeTracker)
		{

		}

		public virtual void CustomChangeTrackingAfterSave(ChangeTracker changeTracker)
		{

		}

		private void TrackRecord(ChangeTracker changeTracker)
		{
			if (changeTracker == null)
			{
				return;
			}

			var when = DateProvider.UTC();

			string userName;
			try
			{
				userName = ContextService?.UserContext?.Name ?? ANONYMOUS_USER;
			}
			catch
			{
				userName = ANONYMOUS_USER;
			}

			var addedLongEntries = changeTracker.Entries<IAuditEntity>().Where(p => p.State == EntityState.Added);
			var addedGuidEntries = changeTracker.Entries<IAuditEntity>().Where(p => p.State == EntityState.Added);
			var updatedLongEntries = changeTracker.Entries<IAuditEntity>().Where(p => p.State == EntityState.Modified);
			var updatedGuidEntries = changeTracker.Entries<IAuditEntity>().Where(p => p.State == EntityState.Modified);

			var addedAuditedLongEntities = addedLongEntries.Select(p => p.Entity);
			foreach (var added in addedAuditedLongEntities)
			{
				added.OnAuditInsert(userName, when);
			}

			var addedAuditedGuidEntities = addedGuidEntries.Select(p => p.Entity);
			foreach (var added in addedAuditedGuidEntities)
			{
				added.OnAuditInsert(userName, when);
			}

			var modifiedAuditedLongEntities = updatedLongEntries.Select(p => p.Entity);
			foreach (var modified in modifiedAuditedLongEntities)
			{
				modified.OnAuditUpdate(userName, when);
			}

			var modifiedAuditedGuidEntities = updatedGuidEntries.Select(p => p.Entity);
			foreach (var modified in modifiedAuditedGuidEntities)
			{
				modified.OnAuditUpdate(userName, when);
			}
		}
	}
}
