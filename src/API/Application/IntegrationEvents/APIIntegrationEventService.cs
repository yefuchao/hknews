using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBus.Abstractions;
using EventBus.Events;

namespace API.Application.IntegrationEvents
{
    public class APIIntegrationEventService : IAPIIntegrationEventService
    {
        private readonly IEventBus _eventBus;

        public APIIntegrationEventService(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void PublishThroughEventBus(IntegrationEvent evt)
        {
            _eventBus.Publish(evt);
        }
    }
}
