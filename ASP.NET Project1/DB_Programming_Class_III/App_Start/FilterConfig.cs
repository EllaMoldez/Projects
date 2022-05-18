using System.Web;
using System.Web.Mvc;

namespace DB_Programming_Class_III
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
