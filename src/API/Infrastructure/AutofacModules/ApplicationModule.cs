using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using API.Application.IntegrationEvents.EventHandling;
using API.Application.IntegrationEvents.Events;
using API.Application.Queries;
using Autofac;
using EventBus.Abstractions;
using HKExNews.Domain.AggregatesModel.StockAggregate;
using HKExNews.Infrastructure.Repositories;

namespace API.Infrastructure.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {
        public string QueriesConnectionString { get; }

        public ApplicationModule(string qconstr)
        {
            QueriesConnectionString = qconstr;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new StockQueries(QueriesConnectionString))
                .As<IStockQueries>()
                .InstancePerLifetimeScope();

            builder.RegisterType<StockRepository>().As<IStockRepository>().InstancePerLifetimeScope();

            builder.Register(c => new NoticeLastDayEventHandler(c.Resolve<IStockRepository>())).As<IIntegrationEventHandler<NoticeLastDayEvent>>().InstancePerLifetimeScope();

            //builder.RegisterAssemblyTypes(typeof(NoticeLastDayEventHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
        }
    }
}
