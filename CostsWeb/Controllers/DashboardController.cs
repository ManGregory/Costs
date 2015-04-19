using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using CostsWeb.Models;
using Microsoft.AspNet.Identity;

namespace CostsWeb.Controllers
{
    public class DashboardController : Controller
    {
        private CostsContext db = new CostsContext();

        private string CurrentUserId
        {
            get { return User.Identity.GetUserId(); }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Chart()
        {
            var months =
                System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("ru")
                    .DateTimeFormat.MonthNames.Take(DateTime.Now.Month).ToArray();
            var sums =
                db.CostsJournal.Where(c => c.UserId == CurrentUserId && c.Date != null && c.Date.Value.Year == DateTime.Now.Year)
                    .GroupBy(c => c.Date.Value.Month)
                    .Select(c1 => c1.Sum(c2 => c2.Sum)).ToArray();
            var myChart = new Chart(width: 600, height: 400)
                .AddTitle("Chart Title")
                .AddSeries(
                    name: "Employee",
                    chartType: "bar",
                    xValue: months,
                    yValues: sums);
            return File(myChart.GetBytes("png"), "image/bytes");
        }
    }
}