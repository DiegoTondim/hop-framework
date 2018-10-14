using System;

namespace Hop.Framework.Domain.Models
{
    public interface IAuditEntity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        DateTime CreatedDate { get; }
        string CreatedBy { get; }
        DateTime? UpdatedDate { get; }
        string UpdatedBy { get; }

        void OnAuditInsert(string user, DateTime when);
        void OnAuditUpdate(string user, DateTime when);
    }
}
