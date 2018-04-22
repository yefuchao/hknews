using HKExNews.Domain.AggregatesModel.StockAggregate;
using HKExNews.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using HKExNews.Domain.AggregatesModel.DateSaveAggregate;

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

        public Task Add(IEnumerable<Records> records)
        {
            return _context.Records.AddRangeAsync(records);
        }

        public bool IsExist(string date)
        {
            return _context.DateSaved.Where(p => p.Date == date).Count() > 0;
        }

        public Task Add(DateSaved date)
        {
            return _context.DateSaved.AddAsync(date);
        }
    }
}
