using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CostsWeb.Helper
{
    internal static class ControllerExtension
    {
        public static string CurrentControllerName(this Controller controller)
        {
            return controller.ControllerContext.RouteData.Values["controller"].ToString();
        }

        public static string CurrentControllerAction(this Controller controller)
        {
            return controller.ControllerContext.RouteData.Values["action"].ToString();
        }

        public static string CreateFlashMessage(this Controller controller, string message, int id)
        {
            return FlashMessageHelper.CreateFlashMessage(
                controller.CurrentControllerName(), controller.CurrentControllerAction(), message, id);
        }
    }
}