using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace SmartHospitalSystem.Extensions
{
    public class SessionAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string _role;

        public SessionAuthorizeAttribute(string role)
        {
            _role = role;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var userRole = session.GetString("UserRole");

            if (string.IsNullOrEmpty(userRole))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            if (userRole != _role)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
