using Hop.Framework.EFCore.Context;
using Hop.Framework.EFCore.Repository;
using Hop.Framework.EFCore.Tests.Infra.Models;

namespace Hop.Framework.EFCore.Tests.Infra.Repositories
{
    public class TabelaTesteReadOnlyWithGuidKeyRepository : ReadOnlyRepositoryWithGuidKeyBase<TabelaTesteWithGuidKey>
    {
        public TabelaTesteReadOnlyWithGuidKeyRepository(HopContextBase context) : base(context)
        {
        }
    }
}