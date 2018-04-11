using API.Application.IntegrationEvents.Events;
using EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Application.IntegrationEvents.EventHandling
{
    public class NoticeLastDayEventHandler : IIntegrationEventHandler<NoticeLastDayEvent>
    {
        public Task Handle(NoticeLastDayEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
