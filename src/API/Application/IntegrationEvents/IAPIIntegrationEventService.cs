using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Application.IntegrationEvents
{
    public interface IAPIIntegrationEventService
    {
        void PublishThroughEventBus(IntegrationEvent evt);
    }
}
