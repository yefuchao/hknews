using HKExNews.Domain.AggregatesModel.DateSaveAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HKExNews.Domain.AggregatesModel.StockAggregate
{
    public interface IStockRepository
    {
        Records Add(Records records);

        Task Add(IEnumerable<Records> records);

        bool IsExist(string date);

        Task Add(DateSaved date);

    }
}
