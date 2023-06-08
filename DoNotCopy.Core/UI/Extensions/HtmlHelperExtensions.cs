using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoNotCopy.Core.UI.Extensions
{
    public static partial class HtmlHelperExtensions
    {
        public static string IsActive(this IHtmlHelper htmlHelper, string controllers = null, string actions = null, string cssClass = "active")
        {
            var routeData = htmlHelper.ViewContext.RouteData;

            if (!routeData.Values.ContainsKey("action") || !routeData.Values.ContainsKey("controller"))
            {
                return "";
            }

            string currentAction = routeData.Values["action"].ToString();
            string currentController = routeData.Values["controller"].ToString();

            if (string.IsNullOrEmpty(actions))
            {
                actions = currentAction;
            }

            if (string.IsNullOrEmpty(controllers))
            {
                controllers = currentController;
            }

            return actions.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).Contains(currentAction) &&
                controllers.Trim().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).Contains(currentController)
                ? cssClass
                : "";
        }

        public static string IsActiveRoute(this IHtmlHelper htmlHelper, string route, string cssClass = "active")
        {
            var routeData = htmlHelper.ViewContext.RouteData;

            if (!routeData.Values.ContainsKey("page"))
            {
                return "";
            }

            string pageRoute = routeData.Values["page"].ToString();

            return route == pageRoute
                ? cssClass
                : "";
        }
    }
}
