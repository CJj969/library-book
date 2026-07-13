using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LibrarySeatReservation.Web.Controllers
{
    public class AdminBaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var endpoint = context.HttpContext.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                base.OnActionExecuting(context);
                return;
            }

            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin")
            {
                context.Result = RedirectToAction("Login");
            }
            base.OnActionExecuting(context);
        }
    }
}
