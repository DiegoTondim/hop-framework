using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Security.Claims;
using System.Security.Principal;

namespace Hop.Api.Server.Core.Auth
{
    public delegate AuthorizationResult DelegateAuthentication(AuthorizationFilterContext authFilterContext, bool authIsEnabled);

    public class AuthFilter : IAuthorizationFilter
    {
        public DelegateAuthentication OnAuthentication { get; set; } = null;
        public bool EnableAuthentication { get; protected set; } = false;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (OnAuthentication == null)
            {
                throw new Exception("The AuthFilter does not have OnAuthentication callback implemented.");
            }

            var authenticationResult = OnAuthentication(context, EnableAuthentication);
            if (authenticationResult.AuthType == AuthorizationResultType.ByPass || 
                authenticationResult.AuthType == AuthorizationResultType.Authenticated || 
                authenticationResult.AuthType == AuthorizationResultType.Authorized)
            {
                if (authenticationResult.Context == null)
                {
                    throw new Exception("The authentication result does not have a valid context.");
                }

                IIdentity identity = new GenericIdentity(authenticationResult.Context.Login);
                context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(identity, authenticationResult.Context.Claims));
                return;
            }

            context.Result = new ContentResult
            {
                StatusCode = 401,
                ContentType = "text/plain",
                Content = authenticationResult.Detail
            };
        }
    }
}
