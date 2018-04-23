using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core
{
    public class HTMLService
    {
        HtmlDocument doc;

        static string _viewState;
        static string _eventvalidation;
        static string _viewStateGenerator = "EC4ACD6F";
        static string _today = DateTime.Now.ToString("yyyyMMdd");
        static string _sortBy = "";
        static string _alertMsg = "";
        static string _ddlShareholdingDay = "20";
        static string _ddlShareholdingMonth = "04";
        static string _ddlShareholdingYear = "2017";


        /// <summary>
        /// 构造函数
        /// </summary>
        public HTMLService()
        {
            doc = new HtmlDocument();
        }

        /// <summary>
        /// 获取POST需要的参数
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public Tuple<string, string> GetStateAndValidation(HtmlDocument doc)
        {
            var nodes = doc.DocumentNode.SelectNodes("//input");

            _viewState = nodes[0].Attributes["value"].Value;

            _eventvalidation = nodes[2].Attributes["value"].Value;

            return new Tuple<string, string>(_viewState, _eventvalidation);
        }

        /// <summary>
        /// 获取POST需要的参数
        /// </summary>
        /// <param name="stream">html数据流</param>
        /// <returns></returns>
        public Tuple<string, string> GetStateAndValidation(Stream stream)
        {
            doc.Load(stream);

            return GetStateAndValidation(doc);
        }

        /// <summary>
        /// 获取日期
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public string GetDate(HtmlDocument doc)
        {
            var date = doc.DocumentNode.SelectNodes("//body/form/div/div")[0].InnerText;

            date = Regex.Replace(date, "\\s", "");

            var index = date.IndexOf(":");

            date = date.Substring(index + 1);

            return date;
        }

        /// <summary>
        /// 获取日期
        /// </summary>
        /// <param name="stream">html数据流</param>
        /// <returns></returns>
        public string GetDate(Stream stream)
        {
            doc.Load(stream);

            return GetDate(doc);
        }

        /// <summary>
        /// 获取股票数据
        /// </summary>
        /// <param name="stream">html对象</param>
        /// <returns></returns>
        public Tuple<string, string> GetStockData(HtmlDocument doc)
        {
            StringBuilder sb = new StringBuilder();

            var row0 = doc.DocumentNode.QuerySelectorAll(".row0 .arial12black");

            foreach (var item in row0)
            {
                sb.Append(Regex.Replace(item.InnerText, "\\s", "") + ";");
            }

            var row1 = doc.DocumentNode.QuerySelectorAll(".row1 .arial12black");

            foreach (var item in row1)
            {
                sb.Append(Regex.Replace(item.InnerText, "\\s", "") + ";");
            }

            var date = doc.DocumentNode.SelectNodes("//body/form/div/div")[0].InnerText;

            date = Regex.Replace(date, "\\s", "");

            var index = date.IndexOf(":");

            date = date.Substring(index + 1);

            return new Tuple<string, string>(date, sb.ToString());
        }

        /// <summary>
        /// 获取股票数据
        /// </summary>
        /// <param name="stream">html数据流</param>
        /// <returns></returns>
        public Tuple<string, string> GetStockData(Stream stream)
        {
            doc.Load(stream);

            return GetStockData(doc);
        }

        /// <summary>
        /// 转换成Json格式
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="date">数据日期</param>
        /// <returns></returns>
        public string ConvertToJson(string data, string date)
        {
            StringBuilder sb = new StringBuilder();

            var array = data.Split(';');

            var index = 0;

            sb.Append("[");

            while (index < array.Length - 1)
            {
                sb.Append("{");
                sb.Append("Code:\"" + Regex.Replace(array[index], "\\s", "") + "\",");
                sb.Append("Stock_Name:\"" + Regex.Replace(array[index + 1], "\\s", "") + "\",");
                sb.Append("Num:" + Regex.Replace(Regex.Replace(array[index + 2], "\\s", ""), ",", "") + ",");
                var rate = Regex.Replace(array[index + 3].Replace('%', ' '), "\\s", "").Length > 0 ? Regex.Replace(array[index + 3].Replace('%', ' '), "\\s", "") : "0";
                sb.Append("Rate:\"" + rate + "\", ");
                sb.Append("Date:\"" + date + "\"");
                sb.Append("}");

                index += 4;

                if (index != array.Length - 4)
                {
                    sb.Append(",");
                }
            }

            sb.Append("]");

            return sb.ToString();
        }

        /// <summary>
        /// 获取今日页面
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public HtmlDocument GetTodayPage(string url)
        {
            HtmlWeb web = new HtmlWeb();

            return web.Load(url);
        }

        /// <summary>
        /// 获取历史页面
        /// </summary>
        /// <param name="viewState"></param>
        /// <param name="eventvalidation"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<Stream> GetHistoryPage(string viewState, string eventvalidation, DateTime date)
        {
            _ddlShareholdingYear = date.Year.ToString();

            _ddlShareholdingMonth = date.Month < 10 ? "0" + date.Month.ToString() : date.Month.ToString();

            _ddlShareholdingDay = date.Day < 10 ? "0" + date.Day.ToString() : date.Day.ToString();

            _viewState = viewState;

            _eventvalidation = eventvalidation;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://www.hkexnews.hk");

                var random = new Random();

                var _x = random.Next(22, 33).ToString();
                var _y = random.Next(11, 17).ToString();

                var content = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string,string>("__VIEWSTATE",_viewState),
                    new KeyValuePair<string, string>("__VIEWSTATEGENERATOR",_viewStateGenerator),
                    new KeyValuePair<string,string>("__EVENTVALIDATION",_eventvalidation),
                    new KeyValuePair<string, string>("today",_today),
                    new KeyValuePair<string,string>("sortBy",_sortBy),
                    new KeyValuePair<string, string>("alertMsg",_alertMsg),
                    new KeyValuePair<string,string>("ddlShareholdingDay",_ddlShareholdingDay),
                    new KeyValuePair<string, string>("ddlShareholdingMonth",_ddlShareholdingMonth),
                    new KeyValuePair<string, string>("ddlShareholdingYear",_ddlShareholdingYear),
                    new KeyValuePair<string, string>("btnSearch.x",_x),
                    new KeyValuePair<string, string>("btnSearch.y",_y),
                });

                var result = await client.PostAsync("/sdw/search/mutualmarket.aspx?t=hk", content);

                var response = await result.Content.ReadAsStreamAsync();

                //var data = GetStockData(response);

                //var page_date = GetDate(response);

                //var json = ConvertToJson(data, page_date);


                return response;
            }

        }
    }
}
