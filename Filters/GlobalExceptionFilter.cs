using System;
using System.Web.Mvc;

namespace TaskManagementSystem1.Filters
{
    public class GlobalExceptionFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var ex = filterContext.Exception;
            System.Diagnostics.Debug.WriteLine($"[Error] {ex.Message}\n{ex.StackTrace}");

            filterContext.ExceptionHandled = true;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                // ✅ Return JSON for AJAX calls
                filterContext.Result = new JsonResult
                {
                    Data = new { success = false, message = "An unexpected error occurred: " + ex.Message },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                // ✅ Show error view for normal MVC page requests
                filterContext.Result = new ViewResult { ViewName = "~/Views/Shared/Error.cshtml" };
            }
        }
    }
}
