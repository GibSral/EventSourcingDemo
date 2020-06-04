using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BankAccount.CoreDomain;
using BankAccount.EventStore;
using EventStore.ClientAPI;
using MediatR;
using MediatR.Pipeline;
using StructureMap;
using StructureMap.Pipeline;

namespace BankAccount.AllAccountsProjection
{
    public class Program
    {
        private static IEventStoreConnection eventStoreConnection;
        private const int DefaultPort = 1113;
        private static readonly SemaphoreSlim ConnectGuard = new SemaphoreSlim(1);

        public static async Task Main(string[] args)
        {
            var storeConnection = await Connect().ConfigureAwait(false);
            using var projectionsDispatcher = new ProjectionsDispatcher(storeConnection, new PositionStoreNullObject(), new IProjection[] { new ShowBalancesProjection() }, LoggerNullObject.Instance);
            projectionsDispatcher.Start();
            Console.ReadKey();
        }

        private static Container ConfigureIoc() => new Container(cfg =>
        {
            cfg.Scan(scanner =>
            {
                scanner.AssembliesFromApplicationBaseDirectory();
                scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
            });

            cfg.For(typeof(IPipelineBehavior<,>)).Add(typeof(RequestPreProcessorBehavior<,>));
            cfg.For(typeof(IPipelineBehavior<,>)).Add(typeof(RequestPostProcessorBehavior<,>));

            cfg.For<IMediator>().LifecycleIs<TransientLifecycle>().Use<Mediator>();
            cfg.For<IBankAccountRepository>().LifecycleIs<SingletonLifecycle>().Use(new BankAccountRepository(Connect));

            cfg.For<ServiceFactory>().Use<ServiceFactory>(ctx => ctx.GetInstance);
        });

        private static async Task<IEventStoreConnection> Connect()
        {
            await ConnectGuard.WaitAsync().ConfigureAwait(false);
            try
            {
                if (eventStoreConnection == null)
                {
                    var settings = ConnectionSettings
                        .Create()
                        .KeepReconnecting()
                        .KeepRetrying()
                        .Build();
                    eventStoreConnection = EventStoreConnection.Create(settings, new IPEndPoint(IPAddress.Loopback, DefaultPort));
                    await eventStoreConnection.ConnectAsync().ConfigureAwait(false);
                }
            }
            finally
            {
                ConnectGuard.Release();
            }
            return eventStoreConnection;
        }
    }
}
