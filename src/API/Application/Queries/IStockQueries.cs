using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Application.Queries
{
    public interface IStockQueries
    {
        Task<IEnumerable<StockNameAndRate>> GetStockRateAsync(string code);

        Task<IEnumerable<StockNameAndAmount>> GetStockAmountAsync(string code);

        Task<IEnumerable<StockItem>> GetDayStockAsync(DateTime date);

        Task<string> GetStockName(string code);

        Task<IEnumerable<StockNameRateChart>> GetStockNameAmount(DateTime date);
    }
}
