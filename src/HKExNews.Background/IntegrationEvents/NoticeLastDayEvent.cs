using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HKExNews.Background.IntegrationEvents
{
    public class NoticeLastDayEvent : IntegrationEvent
    {
        public DateTime Date { get; }

        public NoticeLastDayEvent(DateTime lastDay) => Date = lastDay;
    }
}
