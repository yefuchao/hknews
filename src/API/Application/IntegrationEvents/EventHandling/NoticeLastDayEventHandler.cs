using API.Application.IntegrationEvents.Events;
using Core;
using EventBus.Abstractions;
using HKExNews.Domain.AggregatesModel.StockAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Application.IntegrationEvents.EventHandling
{
    public class NoticeLastDayEventHandler : IIntegrationEventHandler<NoticeLastDayEvent>
    {
        private OperateService _service;

        public NoticeLastDayEventHandler(IStockRepository repository)
        {
            _service = new OperateService(repository);
        }

        public async Task Handle(NoticeLastDayEvent @event)
        {
            await _service.SaveData(@event.Date);
        }
    }
}
