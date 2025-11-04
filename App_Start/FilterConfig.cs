using System.Web;
using System.Web.Mvc;

namespace TaskManagementSystem1
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new TaskManagementSystem1.Filters.GlobalExceptionFilter());
            filters.Add(new TaskManagementSystem1.Filters.LogActionFilter());
        }
    }
}
