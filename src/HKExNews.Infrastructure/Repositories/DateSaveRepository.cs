using HKExNews.Domain.AggregatesModel.DateSaveAggregate;
using HKExNews.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace HKExNews.Infrastructure.Repositories
{
    public class DateSaveRepository : IDateSaveRepository
    {
        private HKExNewsContext _context;

        public IUnitOfWork UnitOfWork
        {
            get { return _context; }
        }

        public DateSaveRepository(HKExNewsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public DateSaved Add(DateSaved dateSaved)
        {
            return _context.Add(dateSaved).Entity;
        }
    }
}
