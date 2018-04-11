using HKExNews.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace HKExNews.Domain.AggregatesModel.DateSaveAggregate
{
    public class DateSaved : IAggregateRoot
    {
        public int Id { get; }

        public string Date { get; private set; }

        public DateSaved(string date)
        {
            Date = date ?? throw new ArgumentNullException(nameof(date));
        }
    }
}
