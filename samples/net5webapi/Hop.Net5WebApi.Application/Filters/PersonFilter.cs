using Hop.Framework.Domain.Repository;

namespace Hop.Net5WebApi.Application.Filters
{
    public class PersonFilter : FilterBase
    {
        public string Name { get; set; }
        public override int PerPage => 10;
    }
}
