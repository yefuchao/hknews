using HKExNews.Domain.AggregatesModel.StockAggregate;
using HKExNews.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace HKExNews.Infrastructure.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly HKExNewsContext _context;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public StockRepository(HKExNewsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Records Add(Records records)
        {
            return _context.Records.Add(records).Entity;
        }
    }
}
