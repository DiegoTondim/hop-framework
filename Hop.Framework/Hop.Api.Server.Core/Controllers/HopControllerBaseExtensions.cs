using Hop.Api.Server.Core.Response;
using Hop.Framework.Domain.Results;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace Hop.Api.Server.Core.Controllers
{
    public static class HopControllerBaseExtensions
    {
        public static IActionResult ReturnOkResponse<T>(this HopControllerBase controller, T content)
        {
            return controller.Ok(new HopApiResponse<T>(content));
        }

        public static IActionResult ReturnNoContentResponse(this HopControllerBase controller)
        {
            return controller.Ok(new HopApiResponse<object>(HttpStatusCode.NoContent));
        }

        public static IActionResult ReturnNotFoundResponse(this HopControllerBase controller, string message, string detail = "")
        {
            return controller.Ok(new HopApiResponse<object>(HttpStatusCode.NotFound, message, detail));
        }

        public static IActionResult ReturnNotFoundResponse(this HopControllerBase controller, Exception ex)
        {
            return controller.Ok(new HopApiResponse<object>(HttpStatusCode.NotFound, ex.Message, ex.InnerException?.Message ?? ex.ToString()));
        }

        public static IActionResult ReturnNotFoundResponse(this HopControllerBase controller, ResultMessage resultMsg)
        {
            return controller.Ok(new HopApiResponse<object>(HttpStatusCode.NotFound, resultMsg));
        }

        public static IActionResult ReturnBadRequestResponse(this HopControllerBase controller, string message, string detail = "")
        {
            return controller.Ok(new HopApiResponse<object>(HttpStatusCode.BadRequest, message, detail));
        }

        public static IActionResult ReturnBadRequestResponse(this HopControllerBase controller, Exception ex)
        {
            return controller.Ok(new HopApiResponse<object>(HttpStatusCode.BadRequest, ex.Message, ex.InnerException?.Message ?? ex.ToString()));
        }

        public static IActionResult ReturnBadRequestResponse(this HopControllerBase controller, ResultMessage resultMsg)
        {
            return controller.Ok(new HopApiResponse<object>(HttpStatusCode.BadRequest, resultMsg));
        }

        public static IActionResult ReturnInternalServerErrorResponse(this HopControllerBase controller, string message, string detail = "")
        {
            return controller.Ok(new HopApiResponse<object>(HttpStatusCode.InternalServerError, message, detail));
        }

        public static IActionResult ReturnInternalServerErrorResponse(this HopControllerBase controller, Exception ex)
        {
            return controller.Ok(new HopApiResponse<object>(HttpStatusCode.InternalServerError, ex.Message, ex.InnerException?.Message ?? ex.ToString()));
        }

        public static IActionResult ReturnInternalServerErrorResponse(this HopControllerBase controller, ResultMessage resultMsg)
        {
            return controller.Ok(new HopApiResponse<object>(HttpStatusCode.InternalServerError, resultMsg));
        }

        public static IActionResult ReturnResponseFromResult(this HopControllerBase controller, Result content)
        {
            var value = content?.Value;
            return content?.Success ?? false
                ? controller.Ok(new HopApiResponse<object>(value))
                : controller.Ok(new HopApiResponse<object>(value, HttpStatusCode.BadRequest, content?.Messages));
        }

        public static IActionResult ReturnResponseFromResult<T>(this HopControllerBase controller, Result<T> content)
        {
            var value = !content.Equals(default(Result<T>)) && !content.Value.Equals(default(T)) ? content.Value : default(T);
            return content?.Success ?? false
                ? controller.Ok(new HopApiResponse<T>(value))
                : controller.Ok(new HopApiResponse<T>(value, HttpStatusCode.BadRequest, content?.Messages));
        }

        public static IActionResult ReturnResponseFromPaginatedData<T>(this HopControllerBase controller, PaginatedData<T> content)
        {
            return controller.Ok(new HopApiResponse<PaginatedData<T>>(content));
        }

        public static IActionResult ReturnResponseFromResultWithPaginatedData<T>(this HopControllerBase controller, ResultWithPaginatedData<T> content)
        {
            return ReturnResponseFromResult(controller, content);
        }
    }
}