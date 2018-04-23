using HKExNews.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace HKExNews.Domain.AggregatesModel.StockAggregate
{
    public class Records : IAggregateRoot
    {
        public int Id { get; }

        public string Code { get; private set; }

        public string Stock_Name { get; private set; }

        public Int64 Num { get; private set; }

        public double Rate { get; private set; }

        public DateTime Date { get; private set; }

        public Records(string code, string stockName, Int64 num, double rate, DateTime date)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Stock_Name = stockName;
            Num = num;
            rate = Rate;
            Date = date;
        }

    }
}
