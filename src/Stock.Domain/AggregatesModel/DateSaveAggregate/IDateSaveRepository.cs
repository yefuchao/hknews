using System;
using System.Collections.Generic;
using System.Text;

namespace HKExNews.Domain.AggregatesModel.DateSaveAggregate
{
    public interface IDateSaveRepository
    {
        DateSaved Add(DateSaved dateSaved);
    }
}
