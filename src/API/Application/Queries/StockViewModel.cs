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
        public string Date { get; set; }

        public string Amount { get; set; }
    }

    public class StockItem
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Amount { get; set; }

        public string Rate { get; set; }
    }
}
