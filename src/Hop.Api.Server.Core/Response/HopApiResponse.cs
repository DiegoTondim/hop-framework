using Hop.Framework.Domain.Results;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Hop.Api.Server.Core.Response
{
    public class HopApiResponse<T>
    {
        public SuplyApiResponseMessage[] Messages { get; protected set; }
        public HttpStatusCode StatusCode { get; }
        public T Content { get; }

        public HopApiResponse(HttpStatusCode status)
        {
            StatusCode = status;
            Content = default(T);
        }

        public HopApiResponse(T content) : this(HttpStatusCode.OK)
        {
            Content = content;
        }

        public HopApiResponse(HttpStatusCode status, string error, string errorDetail = "") : this(status)
        {
            Messages = new[] { new SuplyApiResponseMessage(error, errorDetail) };
        }

        public HopApiResponse(HttpStatusCode status, IEnumerable<ResultMessage> errors) : this(status)
        {
            Messages = errors.Select(p => new SuplyApiResponseMessage(p.Message, p.Type)).ToArray();
        }

        public HopApiResponse(T content, HttpStatusCode status) : this(status)
        {
            Content = content;
        }

        public HopApiResponse(T content, HttpStatusCode status, string error, string errorDetail = "") : this(content, status)
        {
            Messages = new[] { new SuplyApiResponseMessage(error, errorDetail) };
            Content = content;
        }

        public HopApiResponse(T content, HttpStatusCode status, ResultMessage error) : this(content, status)
        {
            Messages = new[] { new SuplyApiResponseMessage(error.Message, error.Type) };
        }

        public HopApiResponse(HttpStatusCode status, ResultMessage error) : this(default(T), status, error)
        {
        }

        public HopApiResponse(T content, HttpStatusCode status, IEnumerable<ResultMessage> errors) : this(content, status)
        {
            Messages = errors.Select(p => new SuplyApiResponseMessage(p.Message, p.Type)).ToArray();
        }
    }

    public class SuplyApiResponseMessage
    {
        public string Content { get; }
        public string Detail { get; }

        public SuplyApiResponseMessage(string content, string detail)
        {
            Content = content;
            Detail = detail;
        }
    }
}
