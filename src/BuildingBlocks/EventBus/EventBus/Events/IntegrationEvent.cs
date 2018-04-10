using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus.Events
{
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.UtcNow;
        }

        public Guid Id { get; }

        public DateTime CreateDate { get; }
    }
}
