using Hop.Framework.Domain.Models;

namespace Hop.Framework.EFCore.Tests.Infra.Models
{
    public class TabelaTesteWithAudit : AuditEntity<long>
    {
        public string Propriedade { get; set; }
    }
}