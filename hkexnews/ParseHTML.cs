using Fizzler.Systems.HtmlAgilityPack;
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
        const string _viewState = "MFwXjMuyVRW4JfBcOeKrytcYpPvgNYCSDdIrFOdMqSYQNZitBloXYJG9pPSzQCChcxAEcwIa1BKjT69bTuKsd2RT3gSdQ+feXZED74vXKO9TNQ+a7+b/UDZ+UoJIjqHvSXCosuNgieJdRVbMnzJwOISK5Z+xHZ+ieASSwGAihr2N7jQnYAPryq7ZsWkhRoOmES7mqA==";
        const string _viewStateGenerator = "EC4ACD6F";
        const string _eventvalidation = "W2oSjvaHIRMGzTpgSbBWJZo9zgbj8nNYZtuFbyNJepjt5Tr/HfbXyuioWwRQiDSNfpclYyUbu1868XlzflZMm0ZM1sM37h2udk25xUXtErZrwZoZmMeLeOQlrmkfSTP/UzN6bJ3b1X6cUjf8aTZHn9R0sHlbeTGGAhxzPHHtOOIFf0ny/vbC5BDKARZw9jeDKBRDs77c7L5Khw8ikYkncF/4dTwZTe/7/FE5pj9lB1OQakuEDVKJ0nEYm8WQC4LcNGy9M3GMI2v3OrQ+SzOtfgFmTf56v1NQnqyWRFiXLkS/IzYL/vCIa7OJrEXfj6GYgWJ42GdUyGotKsGcSvlbEf4NFgfXsFSvvcdfn0dWNUHIVKWLj3tit8DKJwuxgBCH7KlILWM82iDvZxFIpzBslAV5nyqloL+kWoPcKRl5uCMAy/rb53D65KNYgiPjjPrMPGa3G/bkaNBoqWG3gIMrE8VZ2jIceufi5NAVq5id/coWMZTeIzrDgcrg+W5RdNylo3DPuyqAw6n7UhhSIGSD61B6/mLxdgVa0lsWlIQphEEP4oftqmepXYidoROxSzQ3J0u0sIrZeiGB2O+RRt1ysLM9Fcw2W14mF45hfMSPBk84pW17bgb57sCHaC00/agO7yg8YbBkWbDywgY/iXe99GDHVMPf/apd+p0LBRKU1xrJJmai535hcqSHxAAz1yJIvG0PteYzkqO9+IiTrURC1P4asbOfUPkxCW/QSa4SKQRqCjYnh/hMF8idCFL45lH9/mZyAaNUN5dWxeNQu9dpYz48T8LewE+JKixihhjcILpTAeOvgB9lUOPFYNnYWh6tsSGMs/SLemWWNZt9iI3c18qNtiO/1LqwfGCGEbnlMIihgJ3hkbmZe5yJl2BV9OROR9oWdKns86IH4f2mB8Yp9snX+r8eBjKDNHX64dvCCBTq3OjjbMBTY3iHdquQHZXAwtb3SxUPttZJqqTk1AuuGn2Lcp6b76QjR6KLas0t75Ca59gdGdorrZ/SfIRDgIoT89jfrF1uK3+w8B+ou3V+DqzmvgkQYqIFSdaNkiq53chUzRjjkQR5cS2jFfLLVw8JjB6zQKmtxz5dqEz1+yXfaLtcjCEfJxJnQrGgaNoyfkVci2tWE7w1rERs4FB6p9Sk/lEZXE3nV/Bgq4+niEnI5Xg1LCo=";
        const string _today = "20180327";
        const string _sortBy = "";
        const string _alertMsg = "";
        const string _ddlShareholdingDay = "22";
        const string _ddlShareholdingMonth = "03";
        const string _ddlShareholdingYear = "2018";
        const string _btnSearch_x = "36";
        const string _btnSearch_y = "16";

        public async Task SaveData()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://www.hkexnews.hk");

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
                    new KeyValuePair<string, string>("btnSearch.x",_btnSearch_x),
                    new KeyValuePair<string, string>("btnSearch.y",_btnSearch_y),
                });

                var result = await client.PostAsync("/sdw/search/mutualmarket_c.aspx?t=hk", content);

                var response = await result.Content.ReadAsStreamAsync();

                HtmlDocument doc = new HtmlDocument();

                doc.Load(response);

                var t = doc.DocumentNode.QuerySelectorAll(".row0 .arial12black");
                foreach (var item in t)
                {
                    var x = item;
                }

                #region
                //var trs = doc.DocumentNode.SelectNodes("/html[1]/body[1]/div[5]/table[1]/tr");

                //for (int i = 2; i < trs.Count; i++)
                //{
                //    var str = trs[i].InnerHtml;
                //    var after = Regex.Replace(str, "\\s", "");

                //    var t = Regex.Matches(after, "");

                //}
                #endregion

                Console.WriteLine("???");
            }
        }
    }
}
