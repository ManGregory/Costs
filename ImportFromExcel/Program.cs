using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CostsWeb.Models;
using Excel = Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;

namespace ImportFromExcel
{
    class Program
    {
        private static CostsContext db = new CostsContext();
        private static string _userId;

        static void Main(string[] args)
        {
            string filename = @"1.xls";
            _userId = "4c5578d4-347b-431d-be67-55ebe479b4e3";
            var user = db.Users.FirstOrDefault(u => u.Id == _userId);
            string connectionString = "Provider = Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filename + ";" +
                "Extended Properties = Excel 8.0;";

            OleDbDataAdapter dataAdapter = new OleDbDataAdapter("SELECT * FROM [Журнал расходов$]", connectionString);
            DataSet myDataSet = new DataSet();
            dataAdapter.Fill(myDataSet, "1");
            DataTable dataTable = myDataSet.Tables["1"];

            db.CostsJournal.AddRange(
                dataTable.AsEnumerable().Select(GetCostsJournal));

            db.SaveChanges();
        }

        private static CostsJournal GetCostsJournal(DataRow data)
        {
            var dateTime = (DateTime)data["date"];
            var sum = Decimal.Parse(data["summa"].ToString().Replace(".", ","));
            return new CostsJournal
            {
                Date = dateTime,
                UserId = _userId,
                Sum = sum,
                CategoryId = GetCategoryId(data["category"].ToString()),
                SubCategoryId = GetCategoryId(data["subcategory"].ToString()),
                Note = data["note"].ToString()
            };
        }

        private static int? GetCategoryId(string categoryName)
        {
            var category = db.Categories.FirstOrDefault(c => c.Name.ToLower() == categoryName.ToLower());
            return category == null ? (int?) null : category.Id;
        }
    }
}
