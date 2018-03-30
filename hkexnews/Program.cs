using Core;
using hkexnews.Model;
using System;
using System.Threading.Tasks;

namespace hkexnews
{
    class Program
    {
        static void Main(string[] args)
        {

            HTMLService service = new HTMLService();

            const string todayurl = @"http://www.hkexnews.hk/sdw/search/mutualmarket_c.aspx?t=hk";

            var today = service.GetTodayPage(todayurl);

            var data = service.GetStateAndValidation(today);

            string _viewState = data.Item1;
            string _eventvalidation = data.Item2;


            //Task.Run(async () => await parse.SaveData("02"));


            Console.WriteLine("Hello World!");

            Console.ReadLine();
        }

        //TODO 实现自动增长一天
        //TODO 判断返回的当天是否已存在

        //TODO 实现每天自动抓取前一天的数据

        //TODO 订阅模式
        //TODO 生成EXCEL，发送到订阅者邮箱

        //TODO 重构 功能模块分开
    }
}
