using Frontend.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Filters;

public class AuthRedirectExceptionFilter : IExceptionFilter
{
    private readonly IHttpContextAccessor _ctx;

    public AuthRedirectExceptionFilter(IHttpContextAccessor ctx)
    {
        _ctx = ctx;
    }

    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ApiUnauthorizedException)
        {
            var http = _ctx.HttpContext!;
            var returnUrl = http.Request.Path + http.Request.QueryString;
            http.Session.SetString("ReturnUrl", returnUrl);

            context.Result = new RedirectToActionResult("RefreshTokens", "Account", null);
            context.ExceptionHandled = true;
        }
        else if (context.Exception is ApiForbiddenException)
        {
            context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
            context.ExceptionHandled = true;
        }
    }
}
