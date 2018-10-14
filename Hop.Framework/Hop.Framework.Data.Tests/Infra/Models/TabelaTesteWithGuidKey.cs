using System;
using Hop.Framework.Domain.Models;

namespace Hop.Framework.EFCore.Tests.Infra.Models
{
    public class TabelaTesteWithGuidKey : Entity<Guid>
    {
        public string Propriedade { get; set; }
    }
}