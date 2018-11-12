using System.Web;
using System.Web.Mvc;

namespace Sitecore.TechnicalChallenge.WebClient
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
