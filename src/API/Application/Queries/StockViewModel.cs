using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Application.Queries
{
    public class StockNameAndRate
    {
        public string Date { get; set; }

        public string Rate { get; set; }
    }

    public class StockNameAndAmount
    {
        public DateTime Date { get; set; }

        public Int64 Amount { get; set; }
    }

    public class StockDateAmountChart
    {
        public IList<string> Date { get; set; }
        public IList<Int64> Amount { get; set; }
        public string Title { get; set; }
    }

    public class DayStockRateChart
    {
        public List<string> Name { get; set; }

        public List<StockNameRateChart> SeriesData { get; set; }

        public Dictionary<string,bool> Selected { get; set; }
    }

    public class StockNameRateChart
    {
        public string Name { get; set; }

        public double Value { get; set; }
    }

    public class StockItem
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public Int64 Amount { get; set; }

        public Double Rate { get; set; }
    }
}
