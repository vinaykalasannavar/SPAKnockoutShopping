using System.Web;
using System.Web.Mvc;

namespace Vinay.Practice.MVC4.SPAKnockoutShopping
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}