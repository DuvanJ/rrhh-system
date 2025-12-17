using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Proyecto1.Filters
{
    public class AuthorizeRoleAttribute : ActionFilterAttribute
    {
        private readonly string[] _roles;

        public AuthorizeRoleAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var perfil = context.HttpContext.Session.GetString("Perfil");

            if (string.IsNullOrEmpty(perfil) || !_roles.Contains(perfil))
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Home", action = "SinPermisos" })
                );
            }

            base.OnActionExecuting(context);
        }
    }
}


