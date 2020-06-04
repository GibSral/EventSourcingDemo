using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using BankAccount.CoreDomain;
using BankAccount.CoreDomain.Commands;
using BankAccount.CoreDomain.DomainValues;
using BankAccount.CoreDomain.Entities;
using BankAccount.EventStore;
using EventStore.ClientAPI;
using MediatR;
using MediatR.Pipeline;
using StructureMap;
using StructureMap.Pipeline;

namespace BankAccount.Console
{
    public class Program
    {
        private const int DefaultPort = 1113;
        private static IEventStoreConnection eventStoreConnection;
        private static readonly SemaphoreSlim ConnectGuard = new SemaphoreSlim(1);
        private static readonly Guid EmployeeId = Guid.Parse("77093498-5049-4962-afc3-36461f831bca");

        private static async Task Main(string[] args)
        {
            var container = ConfigureIoc();
            var mediator = container.GetInstance<IMediator>();
            var projectionConnection = await MakeNewEventStoreConnection().ConfigureAwait(false);
            var availableAccountsProjection = new AvailableAccountsProjection();
            using var projectionsDispatcher = new ProjectionsDispatcher(projectionConnection,
                new PositionStoreNullObject(),
                new IProjection[] { availableAccountsProjection },
                LoggerNullObject.Instance);
            projectionsDispatcher.Start();
            while (true)
            {
                try
                {
                    System.Console.Clear();
                    System.Console.WriteLine("(c) - Create new bank account");
                    System.Console.WriteLine("(d) - deposit money");
                    System.Console.WriteLine("(w) - withdraw money");
                    var consoleKeyInfo = System.Console.ReadKey();
                    switch (consoleKeyInfo.Key)
                    {
                        case ConsoleKey.C:
                            await CreateAccountAsync(mediator).ConfigureAwait(false);
                            break;
                        case ConsoleKey.D:
                            await Deposit(mediator, availableAccountsProjection);
                            break;
                        case ConsoleKey.W:
                            await Withdraw(mediator, availableAccountsProjection);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                    System.Console.WriteLine("Press any key to continue");
                    System.Console.ReadKey();
                }
            }
        }

        private static Task CreateAccountAsync(IMediator mediator)
        {
            System.Console.Clear();
            System.Console.Write("IBAN: ");
            var iban = Iban.Of(System.Console.ReadLine());
            var createBankAccount = new CreateBankAccount(OId.Of<AccountHolder, Guid>(Guid.NewGuid()),
                OId.Of<Employee, Guid>(EmployeeId),
                Currency.Euro,
                iban,
                TimeStamp.Of(DateTimeOffset.Now.ToUnixTimeSeconds()));
            return mediator.Send(createBankAccount);
        }

        private static Task Deposit(IMediator mediator, AvailableAccountsProjection availableAccountsProjection)
        {
            System.Console.Clear();
            System.Console.Write("IBAN: ");
            var iban = Iban.Of(System.Console.ReadLine());
            System.Console.Write("Amount: ");
            var amount = decimal.Parse(System.Console.ReadLine());
            var bankAccountId = OId.Of<CoreDomain.BankAccount, Guid>(availableAccountsProjection.GetId(iban));
            var depositMoney = new DepositMoney(bankAccountId,
                Transaction.Of(Guid.NewGuid()),
                new Money(amount, Currency.Euro),
                TimeStamp.Of(DateTimeOffset.Now.ToUnixTimeSeconds()));
            return mediator.Send(depositMoney);
        }

        private static Task Withdraw(IMediator mediator, AvailableAccountsProjection availableAccountsProjection)
        {
            System.Console.Clear();
            System.Console.Write("IBAN: ");
            var iban = Iban.Of(System.Console.ReadLine());
            System.Console.Write("Amount: ");
            var amount = decimal.Parse(System.Console.ReadLine());
            var bankAccountId = OId.Of<CoreDomain.BankAccount, Guid>(availableAccountsProjection.GetId(iban));
            var withdrawMoney = new WithdrawMoney(bankAccountId,
                Transaction.Of(Guid.NewGuid()),
                new Money(amount, Currency.Euro),
                TimeStamp.Of(DateTimeOffset.Now.ToUnixTimeSeconds()));
            return mediator.Send(withdrawMoney);
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
                    eventStoreConnection = await MakeNewEventStoreConnection();
                }
            }
            finally
            {
                ConnectGuard.Release();
            }

            return eventStoreConnection;
        }

        private static async Task<IEventStoreConnection> MakeNewEventStoreConnection()
        {
            var settings = ConnectionSettings.Create();
            var connection = EventStoreConnection.Create(settings, new IPEndPoint(IPAddress.Loopback, DefaultPort));
            await connection.ConnectAsync().ConfigureAwait(false);
            return connection;
        }
    }
}