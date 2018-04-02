using Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure;
using System.Linq;

namespace hkexnews
{
    class Program
    {
        static void Main(string[] args)
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

                    Console.WriteLine(startDate.ToString("yyyy年MM月dd日") + "完成");
                });

                Task.Delay(1000);

                startDate = startDate.AddDays(1);
            }

            Console.WriteLine("done this date");

            Console.ReadLine();
        }

        //TODO 实现每天自动抓取前一天的数据

        //TODO 订阅模式
        //TODO 生成EXCEL，发送到订阅者邮箱
    }
}
