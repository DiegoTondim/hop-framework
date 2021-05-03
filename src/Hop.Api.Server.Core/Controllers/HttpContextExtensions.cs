using System;
using System.Net;
using Hop.Api.Server.Core.Response;
using Hop.Framework.Domain.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hop.Api.Server.Core.Controllers
{
    public static class HttpContextExtensions
    {
        public static IActionResult ReturnOkResponse<T>(this HttpContext httpContext, T content)
        {
            return new OkObjectResult(new HopApiResponse<T>(content));
        }

        public static IActionResult ReturnNoContentResponse(this HttpContext httpContext)
        {
            return new OkObjectResult(new HopApiResponse<object>(HttpStatusCode.NoContent));
        }

        public static IActionResult ReturnNotFoundResponse(this HttpContext httpContext, string message, string detail)
        {
            return new OkObjectResult(new HopApiResponse<object>(HttpStatusCode.NotFound, message, detail));
        }

        public static IActionResult ReturnNotFoundResponse(this HttpContext httpContext, Exception ex)
        {
            return new OkObjectResult(new HopApiResponse<object>(HttpStatusCode.NotFound, ex.Message, ex.InnerException?.Message ?? ex.ToString()));
        }

        public static IActionResult ReturnNotFoundResponse(this HttpContext httpContext, ResultMessage resultMsg)
        {
            return new OkObjectResult(new HopApiResponse<object>(HttpStatusCode.NotFound, resultMsg));
        }

        public static IActionResult ReturnBadRequestResponse(this HttpContext httpContext, string message, string detail)
        {
            return new OkObjectResult(new HopApiResponse<object>(HttpStatusCode.BadRequest, message, detail));
        }

        public static IActionResult ReturnBadRequestResponse(this HttpContext httpContext, Exception ex)
        {
            return new OkObjectResult(new HopApiResponse<object>(HttpStatusCode.BadRequest, ex.Message, ex.InnerException?.Message ?? ex.ToString()));
        }

        public static IActionResult ReturnBadRequestResponse(this HttpContext httpContext, ResultMessage resultMsg)
        {
            return new OkObjectResult(new HopApiResponse<object>(HttpStatusCode.BadRequest, resultMsg));
        }

        public static IActionResult ReturnInternalServerErrorResponse(this HttpContext httpContext, string message, string detail)
        {
            return new OkObjectResult(new HopApiResponse<object>(HttpStatusCode.InternalServerError, message, detail));
        }

        public static IActionResult ReturnInternalServerErrorResponse(this HttpContext httpContext, Exception ex)
        {
            return new OkObjectResult(new HopApiResponse<object>(HttpStatusCode.InternalServerError, ex.Message, ex.InnerException?.Message ?? ex.ToString()));
        }

        public static IActionResult ReturnInternalServerErrorResponse(this HttpContext httpContext, ResultMessage resultMsg)
        {
            return new OkObjectResult(new HopApiResponse<object>(HttpStatusCode.InternalServerError, resultMsg));
        }

        public static IActionResult ReturnResponseFromResult(this HttpContext httpContext, Result content)
        {
            var value = content?.Value;
            return content?.Success ?? false
                ? new OkObjectResult(new HopApiResponse<object>(value))
                : new OkObjectResult(new HopApiResponse<object>(value, HttpStatusCode.BadRequest, content?.Messages));
        }

        public static IActionResult ReturnResponseFromResult<T>(this HttpContext httpContext, Result<T> content)
        {
            var value = !content.Equals(default(Result<T>)) && !content.Value.Equals(default(T)) ? content.Value : default(T);
            return content?.Success ?? false
                ? new OkObjectResult(new HopApiResponse<T>(value))
                : new OkObjectResult(new HopApiResponse<T>(value, HttpStatusCode.BadRequest, content?.Messages));
        }

        public static IActionResult ReturnResponseFromPaginatedData<T>(this HttpContext httpContext, PaginatedData<T> content)
        {
            return new OkObjectResult(new HopApiResponse<PaginatedData<T>>(content));
        }

        public static IActionResult ReturnResponseFromResultWithPaginatedData<T>(this HttpContext httpContext, ResultWithPaginatedData<T> content)
        {
            return ReturnResponseFromResult(httpContext, content);
        }
    }
}