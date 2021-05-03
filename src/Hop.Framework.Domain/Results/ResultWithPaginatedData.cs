using System.Collections;
using System.Collections.Generic;

namespace Hop.Framework.Domain.Results
{
    public class ResultWithPaginatedData<TViewModel> : Result<PaginatedData<TViewModel>>
    {
        public ResultWithPaginatedData(PaginatedData<TViewModel> data) : base(true)
        {
            this.AddValue(data);
        }

        public ResultWithPaginatedData(int currentPage, int total, int perPage, IEnumerable<TViewModel> data) : base(true)
        {
            this.AddValue(new PaginatedData<TViewModel>(currentPage, total, perPage, data));
        }
    }

    public class PaginatedData<TViewModel>
    {
        public int CurrentPage { get; set; }
        public int Total { get; set; }
        public int PerPage { get; set; }
        public IEnumerable<TViewModel> Data { get; set; }

        public PaginatedData(int currentPage, int total, int perPage, IEnumerable<TViewModel> data)
        {
            CurrentPage = currentPage;
            Total = total;
            PerPage = perPage;
            Data = data;
        }
    }
}
