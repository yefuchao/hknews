using Fizzler.Systems.HtmlAgilityPack;
using hkexnews.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace hkexnews
{

    public class ParseHTML
    {
        HKNewsContext db;

        public ParseHTML()
        {
            db = new HKNewsContext();
        }

        static string _viewState = "";
        static string _viewStateGenerator = "EC4ACD6F";
        static string _eventvalidation = "";
        static string _today = "20180330";
        static string _sortBy = "";
        static string _alertMsg = "";
        static string _ddlShareholdingDay = "20";
        static string _ddlShareholdingMonth = "04";
        static string _ddlShareholdingYear = "2017";

        const string url = @"http://www.hkexnews.hk/sdw/search/mutualmarket_c.aspx?t=hk";



        public void GetHtml()
        {
            try
            {
                HtmlWeb web = new HtmlWeb();

                var htmlDoc = web.Load(url);

                var nodes = htmlDoc.DocumentNode.SelectNodes("//input");

                _viewState = nodes[0].Attributes["value"].Value;

                _eventvalidation = nodes[2].Attributes["value"].Value;

                Console.WriteLine("done");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task SaveData(string day)
        {
            _ddlShareholdingDay = day;

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

                var result = await client.PostAsync("/sdw/search/mutualmarket_c.aspx?t=hk", content);

                var response = await result.Content.ReadAsStreamAsync();

                HtmlDocument doc = new HtmlDocument();

                doc.Load(response);

                var nodes = doc.DocumentNode.SelectNodes("//input");

                _viewState = nodes[0].Attributes["value"].Value;

                _eventvalidation = nodes[2].Attributes["value"].Value;

                var date = doc.DocumentNode.SelectNodes("//body/form/div/div")[0].InnerText;

                date = Regex.Replace(date, "\\s", "");

                var index = date.IndexOf(":");

                date = date.Substring(index + 1);

                var t = doc.DocumentNode.QuerySelectorAll(".row0 .arial12black");

                StringBuilder sb = new StringBuilder();

                foreach (var item in t)
                {
                    sb.Append(Regex.Replace(item.InnerText, "\\s", "") + ";");
                }

                var x = doc.DocumentNode.QuerySelectorAll(".row1 .arial12black");

                foreach (var item in t)
                {
                    sb.Append(Regex.Replace(item.InnerText, "\\s", "") + ";");
                }

                var array = sb.ToString().Split(";");

                var postion = 0;

                StringBuilder builder = new StringBuilder();

                while (postion < array.Length - 1)
                {
                    Records record = new Records
                    {
                        Code = array[postion],
                        Date = date,
                        Stock_Name = array[postion + 1],
                        Rate = array[postion + 3],
                        Num = array[postion + 2]
                    };

                    db.Records.Add(record);

                    builder.Append("id:" + array[postion] + ",name:" + array[postion + 1] + ",num:" + array[postion + 2] + ",rate:" + array[postion + 3] + ";");
                    postion += 4;
                }

                db.SaveChanges();

                builder.Append("}");

                Console.WriteLine(builder.ToString());

                Console.WriteLine("done");

            }
        }
    }
}
