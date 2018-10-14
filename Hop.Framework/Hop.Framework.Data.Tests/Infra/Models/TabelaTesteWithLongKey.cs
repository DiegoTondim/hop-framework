using Hop.Framework.Domain.Models;

namespace Hop.Framework.EFCore.Tests.Infra.Models
{
    public class TabelaTesteWithLongKey : Entity<long>
    {
        public string Propriedade { get; set; }
    }
}