using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Proyecto1.Filters
{
    public class ValidateSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session.GetString("Usuario");

            if (string.IsNullOrEmpty(session))
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Login", action = "Index" })
                );
            }

            base.OnActionExecuting(context);
        }
    }
}
