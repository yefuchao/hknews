using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using API.Application.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/Stock")]
    public class StockController : Controller
    {
        private IStockQueries _stockQueries;

        public StockController(IStockQueries stockQueries)
        {
            _stockQueries = stockQueries;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(DayStockRateChart), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetStockByDay(string datestr = null)
        {
            try
            {
                DateTime.TryParse(datestr, out DateTime date);

                date = DateTime.MinValue == date ? new DateTime(2018, 03, 29) : date;

                var list = await _stockQueries.GetStockNameAmount(date);

                var daystockRate = ConvertToStockRateChartData(list);

                return Ok(daystockRate);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        private DayStockRateChart ConvertToStockRateChartData(IEnumerable<StockNameRateChart> stocks)
        {
            DayStockRateChart dayStockRate = new DayStockRateChart()
            {
                Selected = new Dictionary<string, bool>(),
                Name = new List<string>(),
                SeriesData = stocks.ToList()
            };

            List<string> name = new List<string>();

            if (stocks.Count() < 1)
            {
                return dayStockRate;

            }

            foreach (var item in stocks)
            {
                name.Add(item.Name);
                dayStockRate.Selected.Add(item.Name, item.Value >= 20);
            }

            dayStockRate.Name = name;

            return dayStockRate;
        }

        [HttpGet]
        [Route("amount/{code}")]
        [ProducesResponseType(typeof(StockDateAmountChart), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetStockNameAndAmount(string code)
        {
            try
            {
                var list = await _stockQueries.GetStockAmountAsync(code);

                var title = await _stockQueries.GetStockName(code);

                var stockDateAmountChart = new StockDateAmountChart()
                {
                    Amount = new List<Int64>(),
                    Date = new List<string>(),
                    Title = title
                };

                foreach (var item in list)
                {
                    stockDateAmountChart.Date.Add(item.Date.ToString("yyyy/MM/dd"));
                    stockDateAmountChart.Amount.Add(item.Amount);
                }

                return Ok(stockDateAmountChart);

            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("rate/{code}")]
        [ProducesResponseType(typeof(IEnumerable<StockNameAndRate>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetStockNameAndRate(string code)
        {
            try
            {
                var list = await _stockQueries.GetStockRateAsync(code);

                return Ok(list);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}