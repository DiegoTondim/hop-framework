using System;

namespace Hop.Framework.Domain.Models
{
    public interface ISoftDelete
    {
        bool Removed { get; }
        DateTime? RemovedDate { get; }
        string RemovedBy { get; }

        void Remove(string user);
        void Remove(string user, DateTime date);
    }
}
