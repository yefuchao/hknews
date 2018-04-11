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

        public string Num { get; private set; }

        public string Rate { get; private set; }

        public string Date { get; private set; }

        public Records(string code, string stockName, string num, string rate, string date)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Stock_Name = stockName;
            Num = num;
            rate = Rate;
            Date = date;
        }

    }
}
