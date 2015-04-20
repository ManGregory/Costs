using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI;
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

        public ActionResult Index(DateTime? dateFrom, DateTime? dateTo, string chartType = "bar")
        {           
            ViewBag.ChartTypes = new SelectList(
                new[]
                {
                    "Area", "Bar", "BoxPlot", "Bubble", "Candlestick", "Column", "Doughnut", "ErrorBar", "FastLine",
                    "FastPoint", "Funnel", "Kagi", "Line", "Pie", "Point", "PointAndFigure", "Polar", "Pyramid", "Radar",
                    "Range", "RangeBar", "RangeColumn", "Renko", "Spline", "SplineArea", "SplineRange", "StackedArea",
                    "StackedArea100", "StackedBar", "StackedBar100", "StackedColumn", "StackedColumn100", "StepLine",
                    "Stock", "ThreeLineBreak"
                });
            TempData["DateFrom"] = dateFrom ?? new DateTime(DateTime.Now.Year, 1, 1);
            TempData["DateTo"] = dateTo ?? DateTime.Now;
            TempData["CurrentChartType"] = chartType;
            return View();
        }

        public ActionResult Chart()
        {
            var chartType = TempData["CurrentChartType"] ?? "bar";
            var dateFrom = TempData["DateFrom"] == null ? new DateTime(DateTime.Now.Year, 1, 1) : (DateTime) TempData["DateFrom"];
            var dateTo = TempData["DateTo"] == null ? DateTime.Now : (DateTime) TempData["DateTo"];
            var months =
                System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("ru")
                    .DateTimeFormat.MonthNames.Take(dateTo.Month).ToArray();
            var sums =
                db.CostsJournal.Where(c => c.UserId == CurrentUserId && c.Date != null && c.Date.Value >= dateFrom && c.Date.Value <= dateTo)
                    .GroupBy(c => c.Date.Value.Month)
                    .Select(c1 => c1.Sum(c2 => c2.Sum)).ToArray();
            var myChart = new Chart(width: 600, height: 400, theme: ChartTheme.Green)
                .AddTitle("Chart Title")
                .AddSeries(
                    name: "Employee",
                    chartType: chartType.ToString(),
                    xValue: months,
                    yValues: sums);
            return File(myChart.GetBytes("png"), "image/bytes");
        }
    }
}