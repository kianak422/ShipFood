using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ShipFood.API.Filters
{
    public class AdminAuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var username = context.HttpContext.Session.GetString("Username");
            // var role = context.HttpContext.Session.GetString("Role"); // Need to implement Role storage in Login

            if (string.IsNullOrEmpty(username))
            {
                context.Result = new RedirectToActionResult("Login", "Home", new { area = "" });
                return;
            }

            // Simple check (in real app, check DB for 'Admin' role)
            if (username != "admin")
            {
                // context.Result = new RedirectToActionResult("Index", "Home", new { area = "" });
            }

            base.OnActionExecuting(context);
        }
    }
}
