using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList.Mvc;

namespace CostsWeb.Helper
{
    public static class FlashMessageHelper
    {
        public static string CreateFlashMessage(string controller, string action, string message, int id)
        {
            var flashMessage = message;
            if ((action == "Create") || (action == "Edit") || (action == "UndoDelete"))
            {
                flashMessage = string.Format("{0}. {1} | {2}", message,
                    string.Format("<a href=\"/{0}/{1}/{2}\">{3}</a>", controller, "Details", id, "Просмотр"),
                    string.Format("<a href=\"/{0}/{1}/{2}\">{3}</a>", controller, "Edit", id, "Редактировать"));
            } 
            else if (action == "Delete")
            {
                flashMessage = string.Format("{0}. {1}", message,
                    string.Format("<a href=\"/{0}/{1}/{2}\">{3}</a>", controller, "UndoDelete", id, "Восстановить"));                
            }
            return flashMessage;
        }        
    }
}