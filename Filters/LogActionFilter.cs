using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace TaskManagementSystem1.Filters
{
    public class LogActionFilter : ActionFilterAttribute
    {
        private Stopwatch stopwatch;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            stopwatch = Stopwatch.StartNew();
            filterContext.HttpContext.Items["ActionStartTime"] = DateTime.UtcNow;
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            stopwatch?.Stop();
            var elapsed = stopwatch?.ElapsedMilliseconds ?? 0;
            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var action = filterContext.ActionDescriptor.ActionName;
            // For simplicity, write to Debug. In real app, use proper logging.
            Debug.WriteLine($"Action Executed: {controller}/{action} took {elapsed} ms");
            base.OnActionExecuted(filterContext);
        }
    }
}
