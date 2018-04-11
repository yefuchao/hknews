using System;
using System.Collections.Generic;
using System.Text;

namespace HKExNews.Domain.AggregatesModel.StockAggregate
{
    public interface IStockRepository
    {
        Records Add(Records records);


    }
}
