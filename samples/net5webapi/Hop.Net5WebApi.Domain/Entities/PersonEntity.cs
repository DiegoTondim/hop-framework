using Hop.Framework.Domain.Models;
using System;

namespace Hop.Net5WebApi.Domain.Entities
{
    public class PersonEntity : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsNew => Id != Guid.Empty;

        public PersonEntity(string name)
        {
            Name = name;
        }
    }
}
