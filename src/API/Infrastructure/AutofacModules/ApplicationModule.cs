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

            builder.RegisterAssemblyTypes(typeof(NoticeLastDayEventHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
        }
    }
}
