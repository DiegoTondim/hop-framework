using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hop.Framework.Domain.Models
{
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }
        public bool IsNew => Id.Equals(default(TPrimaryKey));

        protected Entity()
        {
            Id = default(TPrimaryKey);
        }
        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity<TPrimaryKey>;

            if (ReferenceEquals(this, compareTo))
                return true;

            return !(compareTo is null) && Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity<TPrimaryKey> a, Entity<TPrimaryKey> b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity<TPrimaryKey> a, Entity<TPrimaryKey> b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + " [Id=" + Id + "]";
        }
    }
}
