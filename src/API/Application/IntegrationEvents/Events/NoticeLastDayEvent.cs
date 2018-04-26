using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Application.IntegrationEvents.Events
{
    public class NoticeLastDayEvent : IntegrationEvent
    {
        //Question:需要set，否则JsonConvert.DeserializeObject无法赋值
        public DateTime Date { get; set; }

        public NoticeLastDayEvent(DateTime lastDay) => Date = lastDay;
    }
}
