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
        [ProducesResponseType(typeof(IEnumerable<StockItem>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetStockByDay(string datestr = null)

        {
            try
            {
                DateTime.TryParse(datestr, out DateTime date);

                date = DateTime.MinValue == date ? new DateTime(2018, 03, 29) : date;

                var list = await _stockQueries.GetDayStockAsync(date);

                return Ok(list);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("amount/{code}")]
        [ProducesResponseType(typeof(IEnumerable<StockNameAndAmount>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetStockNameAndAmount(string code)
        {
            try
            {
                var list = await _stockQueries.GetStockAmountAsync(code);

                return Ok(list);

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