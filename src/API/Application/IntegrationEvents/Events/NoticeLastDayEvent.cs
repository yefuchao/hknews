using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Application.IntegrationEvents.Events
{
    public class NoticeLastDayEvent : IntegrationEvent
    {
        public string Date { get; }

        public NoticeLastDayEvent(string lastDay) => Date = lastDay;
    }
}
