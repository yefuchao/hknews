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
using hkexnews.Mysql;
using hkexnews.Model;
using System.Globalization;

namespace hkexnews
{
    class Program
    {
        static void Main(string[] args)
        {
            SaveNames();

            Console.WriteLine("Job Done");

            Console.ReadLine();
        }

        public static void SaveNames()
        {
            HTMLService service = new HTMLService();

            const string todayurl = @"http://www.hkexnews.hk/sdw/search/mutualmarket_c.aspx?t=hk";

            var today = service.GetTodayPage(todayurl);

            var data = service.GetStockData(today);

            var date = DateTime.ParseExact(data.Item1, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var json = service.ConvertToJson(data.Item2, date.ToString("yyyy/MM/dd"));

            using (var db = new HkNewsContext())
            {
                var list = JsonConvert.DeserializeObject<List<Records>>(json);
                var names = db.Names.ToList();
                foreach (var item in list)
                {
                    if (names != null)
                    {
                        var name = names.Where(p => p.Code == item.Code).FirstOrDefault();
                        name.CN = item.Stock_Name;
                        db.Entry(name).State = EntityState.Modified;
                    }
                }

                db.SaveChanges();
            }
            Console.Write("finish");
        }

        public static void SaveDataToMysql()
        {
            List<DateSaved> records;

            using (var sqlserver = new HKNewsContext())
            {
                records = sqlserver.DateSaved.ToList();
            }

            using (var mysql = new HkNewsContext())
            {
                mysql.Database.EnsureCreated();

                mysql.DateSaved.AddRange(records);

                mysql.SaveChanges();
            }
        }

        public static void SaveCurrentPage()
        {
            HTMLService service = new HTMLService();

            const string todayurl = @"http://www.hkexnews.hk/sdw/search/mutualmarket_c.aspx?t=hk";

            var today = service.GetTodayPage(todayurl);

            var data = service.GetStockData(today);

            var date = Convert.ToDateTime(data.Item1);

            var json = service.ConvertToJson(data.Item2, data.Item1);

            using (var db = new HkNewsContext())
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

        public static void SaveHistoryDataToMysql()
        {
            HTMLService service = new HTMLService();

            const string todayurl = @"http://www.hkexnews.hk/sdw/search/mutualmarket.aspx?t=hk";

            DateTime startDate = new DateTime(2018, 3, 3);

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

                    var date = DateTime.ParseExact(r.Item1, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    var json = service.ConvertToJson(r.Item2, date.ToString());

                    using (var db = new HkNewsContext())
                    {
                        if (db.DateSaved.Where(p => p.Date == r.Item1).Count() == 0)
                        {
                            var list = JsonConvert.DeserializeObject<List<Records>>(json);

                            db.Records.AddRange(list);

                            db.DateSaved.Add(new DateSaved { Date = r.Item1 });

                            db.SaveChanges();
                        }
                    }

                    Console.WriteLine(r.Item1 + "完成");
                });

                Task.Delay(2000);

                startDate = startDate.AddDays(1);
            }

            Console.WriteLine("done this date");

            Console.ReadLine();
        }

        public static void SaveHistoryData()
        {
            HTMLService service = new HTMLService();

            const string todayurl = @"http://www.hkexnews.hk/sdw/search/mutualmarket.aspx?t=hk";

            DateTime startDate = new DateTime(2018, 1, 1);

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

                    var date = Convert.ToDateTime(r.Item1);

                    var json = service.ConvertToJson(r.Item2, r.Item1);

                    using (var db = new HKNewsContext())
                    {
                        if (db.DateSaved.Where(p => p.Date == r.Item1).Count() == 0)
                        {
                            var list = JsonConvert.DeserializeObject<List<Records>>(json);

                            db.Records.AddRange(list);

                            db.DateSaved.Add(new DateSaved { Date = r.Item1 });

                            db.SaveChanges();
                        }
                    }

                    Console.WriteLine(r.Item1 + "完成");
                });

                Task.Delay(2000);

                startDate = startDate.AddDays(1);
            }

            Console.WriteLine("done this date");

            Console.ReadLine();
        }

        //public static void GenerateExcelAll(int year, int month)
        //{
        //    var targetMonth = (month < 10 ? "0" + month : month.ToString()) + "/" + year.ToString();

        //    var targetPath = AppDomain.CurrentDomain.BaseDirectory + DateTime.Now.ToFileTimeUtc() + ".xls";

        //    DataSet set = new DataSet();

        //    using (var db = new HKNewsContext())
        //    {
        //        var stockData = db.Records.OrderBy(p => p.Id).Where(P => P.Date.Contains(targetMonth)).GroupBy(p => p.Date).ToList();

        //        foreach (var item in stockData)
        //        {
        //            DataTable sourceTable = new DataTable(ConverToTableName(item.Key));

        //            sourceTable.Columns.Add("股票代号");
        //            sourceTable.Columns.Add("股份名称");
        //            sourceTable.Columns.Add("于中央结算系统的持股量");
        //            sourceTable.Columns.Add("占已发行股份百分比");

        //            foreach (var perStock in item)
        //            {
        //                var row = sourceTable.NewRow();

        //                row["股票代号"] = perStock.Code;
        //                row["股份名称"] = perStock.Stock_Name;
        //                row["于中央结算系统的持股量"] = perStock.Num;
        //                row["占已发行股份百分比"] = perStock.Rate;

        //                sourceTable.Rows.Add(row);
        //            }

        //            set.Tables.Add(sourceTable);
        //        }

        //        var excelStream = ExcelService.RenderDataSetToExcel(set);

        //        ExcelService.WriteStreamToFile(ExcelService.StreamToByteArray(excelStream), targetPath);
        //    }
        //}

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
            //var targetDate = date.ToString("dd/MM/yyyy");

            var targetPath = AppDomain.CurrentDomain.BaseDirectory + date.ToString("yyyyMMdd") + DateTime.Now.ToFileTimeUtc() + ".xls";

            using (var db = new HKNewsContext())
            {
                var stockData = db.Records.Where(p => p.Date == date).OrderBy(p => p.Code).ToList();

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

    }
}
