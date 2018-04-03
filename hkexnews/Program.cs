using Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure;
using System.Linq;
using System.Data;
using hkexnews.Services;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace hkexnews
{
    class Program
    {
        static void Main(string[] args)
        {
            GenerateExcelAll(2017,12);

            Console.WriteLine("job done");

            Console.ReadLine();
        }

        public static void SaveCurrentPage()
        {
            HTMLService service = new HTMLService();

            const string todayurl = @"http://www.hkexnews.hk/sdw/search/mutualmarket_c.aspx?t=hk";

            var today = service.GetTodayPage(todayurl);

            var data = service.GetStockData(today);

            var json = service.ConvertToJson(data.Item2, data.Item1);

            using (var db = new HKNewsContext())
            {
                if (db.DateSaved.Where(p => p.Date == data.Item1).Count() == 0)
                {
                    var list = JsonConvert.DeserializeObject<List<Records>>(json);

                    db.Records.AddRange(list);

                    db.DateSaved.Add(new Model.DateSaved { Date = data.Item1 });

                    db.SaveChanges();
                }
            }

            Console.WriteLine(data.Item1 + "完成");
        }

        public static void SaveHistoryData()
        {
            HTMLService service = new HTMLService();

            const string todayurl = @"http://www.hkexnews.hk/sdw/search/mutualmarket_c.aspx?t=hk";

            DateTime startDate = new DateTime(2017, 12, 12);

            while (startDate.Date != DateTime.Now.Date)
            {
                var today = service.GetTodayPage(todayurl);

                var data = service.GetStateAndValidation(today);

                string _viewState = data.Item1;
                string _eventvalidation = data.Item2;

                service.GetHistoryPage(_viewState, _eventvalidation, startDate).ContinueWith((obj) =>
                {
                    var response = obj.Result;

                    var r = service.GetStockData(response);

                    var json = service.ConvertToJson(r.Item2, r.Item1);

                    using (var db = new HKNewsContext())
                    {
                        if (db.DateSaved.Where(p => p.Date == r.Item1).Count() == 0)
                        {
                            var list = JsonConvert.DeserializeObject<List<Records>>(json);

                            db.Records.AddRange(list);

                            db.DateSaved.Add(new Model.DateSaved { Date = r.Item1 });

                            db.SaveChanges();
                        }
                    }

                    Console.WriteLine(data.Item1 + "完成");
                });

                Task.Delay(1000);

                startDate = startDate.AddDays(1);
            }

            Console.WriteLine("done this date");

            Console.ReadLine();
        }

        public static void GenerateExcelAll(int year, int month)
        {
            var targetMonth = (month < 10 ? "0" + month : month.ToString()) + "/" + year.ToString();

            var targetPath = AppDomain.CurrentDomain.BaseDirectory + DateTime.Now.ToFileTimeUtc() + ".xls";

            DataSet set = new DataSet();

            using (var db = new HKNewsContext())
            {
                var stockData = db.Records.OrderBy(p => p.Id).Where(P => P.Date.Contains(targetMonth)).GroupBy(p => p.Date).ToList();

                foreach (var item in stockData)
                {
                    DataTable sourceTable = new DataTable(ConverToTableName(item.Key));

                    sourceTable.Columns.Add("股票代号");
                    sourceTable.Columns.Add("股份名称");
                    sourceTable.Columns.Add("于中央结算系统的持股量");
                    sourceTable.Columns.Add("占已发行股份百分比");

                    foreach (var perStock in item)
                    {
                        var row = sourceTable.NewRow();

                        row["股票代号"] = perStock.Code;
                        row["股份名称"] = perStock.Stock_Name;
                        row["于中央结算系统的持股量"] = perStock.Num;
                        row["占已发行股份百分比"] = perStock.Rate;

                        sourceTable.Rows.Add(row);
                    }

                    set.Tables.Add(sourceTable);
                }

                var excelStream = ExcelService.RenderDataSetToExcel(set);

                ExcelService.WriteStreamToFile(ExcelService.StreamToByteArray(excelStream), targetPath);
            }
        }

        public static string ConverToTableName(string str)
        {
            var array = str.Split("/");

            StringBuilder sb = new StringBuilder();

            for (int i = array.Length - 1; i >= 0; i--)
            {
                sb.Append(array[i]);
            }

            return sb.ToString();

        }

        public static void GenereateExcelByDay(DateTime date)
        {
            var targetDate = date.ToString("dd/MM/yyyy");

            var targetPath = AppDomain.CurrentDomain.BaseDirectory + date.ToString("yyyyMMdd") + DateTime.Now.ToFileTimeUtc() + ".xls";

            using (var db = new HKNewsContext())
            {
                var stockData = db.Records.Where(p => p.Date == targetDate).OrderBy(p => p.Code).ToList();

                DataTable sourceTable = new DataTable(date.ToString("yyyyMMdd"));

                sourceTable.Columns.Add("股票代号");
                sourceTable.Columns.Add("股份名称");
                sourceTable.Columns.Add("于中央结算系统的持股量");
                sourceTable.Columns.Add("占已发行股份百分比");

                foreach (var item in stockData)
                {
                    var row = sourceTable.NewRow();

                    row["股票代号"] = item.Code;
                    row["股份名称"] = item.Stock_Name;
                    row["于中央结算系统的持股量"] = item.Num;
                    row["占已发行股份百分比"] = item.Rate;

                    sourceTable.Rows.Add(row);
                }

                var excelStream = ExcelService.GenerateDayExcel(sourceTable);

                ExcelService.WriteStreamToFile(ExcelService.StreamToByteArray(excelStream), targetPath);
            }


        }

        public static void GenerateExcelByCode(string Code)
        {

        }

        //TODO 实现每天自动抓取前一天的数据
        //TODO 订阅模式
        //TODO 生成EXCEL，发送到订阅者邮箱
    }
}
