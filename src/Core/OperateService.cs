using HKExNews.Domain.AggregatesModel.DateSaveAggregate;
using HKExNews.Domain.AggregatesModel.StockAggregate;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class OperateService
    {
        private HTMLService _Html;
        private IStockRepository _repository;
        private const string todayurl = @"http://www.hkexnews.hk/sdw/search/mutualmarket_c.aspx?t=hk";

        public OperateService(IStockRepository repository)
        {
            _Html = new HTMLService();
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task SaveData(DateTime startDate)
        {
            while (startDate != DateTime.Now.Date)
            {
                var todayPage = _Html.GetTodayPage(todayurl);

                var data = _Html.GetStateAndValidation(todayPage);

                string _viewState = data.Item1;

                string _eventvalidation = data.Item2;

                await _Html.GetHistoryPage(_viewState, _eventvalidation, startDate).ContinueWith(async (obj) =>
                {
                    var response = obj.Result;
                    var stockData = _Html.GetStockData(response);
                    var json = _Html.ConvertToJson(stockData.Item2, stockData.Item1);

                    if (!_repository.IsExist(stockData.Item1))
                    {
                        var records = JsonConvert.DeserializeObject<List<Records>>(json);
                        await _repository.Add(records);
                        //TODO modify:use meditor tp notice new date data saved
                        await _repository.Add(new DateSaved(stockData.Item1));
                    }
                });

                await Task.Delay(2000);

                startDate = startDate.AddDays(1);
            }
        }
    }
}
