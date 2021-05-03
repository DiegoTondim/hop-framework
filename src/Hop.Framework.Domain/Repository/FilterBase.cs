namespace Hop.Framework.Domain.Repository
{
    public abstract class FilterBase
    {
        public abstract int PerPage { get; }
        public int CurrentPage { get; set; }
        public string Column { get; set; }
        public Order Order { get; set; }
    }

    public enum Order
    {
        Asc,
        Desc
    }
}
