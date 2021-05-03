using System;

namespace Hop.Framework.Domain.Models
{
    public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
        bool IsNew { get; }
    }
}
