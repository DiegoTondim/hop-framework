using System;

namespace Hop.Framework.Domain.Models
{
    public abstract class AuditEntity<TPrimaryKey> : Entity<TPrimaryKey>, IAuditEntity<TPrimaryKey>
    {
        public DateTime CreatedDate { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime? UpdatedDate { get; private set; }
        public string UpdatedBy { get; private set; }

        protected AuditEntity()
        {
            CreatedDate = DateTime.Now;
        }

        public void OnAuditInsert(string user, DateTime when)
        {
            this.CreatedBy = user;
            this.CreatedDate = when;
        }

        public void OnAuditUpdate(string user, DateTime when)
        {
            this.UpdatedBy = user;
            this.UpdatedDate = when;
        }
    }

    public abstract class AuditEntityWithSoftDelete<TPrimaryKey> : AuditEntity<TPrimaryKey>, ISoftDelete
    {
        public bool Removed { get; private set; }
        public DateTime? RemovedDate { get; private set; }
        public string RemovedBy { get; private set; }
        public void Remove(string user)
        {
            Removed = true;
            RemovedDate = DateTime.Now;
            RemovedBy = user;
        }

        public void Remove(string user, DateTime date)
        {
            Removed = true;
            RemovedDate = date;
            RemovedBy = user;
        }
    }
}
