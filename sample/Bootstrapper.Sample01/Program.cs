using System;
using System.Threading.Tasks;
using Bootstrapper.Abstractions;
using Bootstrapper.Sample.LogIntercept;
using DryIoc;
using MediatR;
using Serilog;

namespace Bootstrapper.Sample01
{
    class Program
    {
        static void Main(string[] args) { new Program().Start(args); }

        private void Start(string[] strings)
        {
            using var container = new Container();
            Bootstrap.Using(container)
                .WithLooseRegistration(registrator =>
                {
                    var config = new LoggerConfiguration();
                    
                    config.WriteTo.Console();
                    config.MinimumLevel.Verbose();

                    Log.Logger = config.CreateLogger();
                    registrator.Register<LogInterceptor>();
                })
                .WithLooseRegistration(registrator =>
                {
                    registrator.RegisterDelegate<ServiceFactory>(r => r.Resolve);
                    registrator.RegisterMany(new[] { typeof(IMediator).GetAssembly(), typeof(Ping).GetAssembly() }, Registrator.Interfaces);
                    registrator.Intercept<IMediator, LogInterceptor>();
                    registrator.Intercept<INotification, LogInterceptor>();
                    registrator.Intercept<INotificationHandler<Ping>, LogInterceptor>();
                })
                .WithAppLevelExceptionHandling(AppLevelException)
                .Run(Execute);
        }

        private async Task Execute(IResolver resolver)
        {
            //Send Ping without response
            var mediator = resolver.Resolve<IMediator>();
            await mediator.Publish(new Ping("Hello world"));
            System.Console.ReadKey();
        }

        private void AppLevelException(object sender, UnhandledExceptionEventArgs arg)
        {
            //Handle App Level exception here
            var ex = (Exception)arg.ExceptionObject;
            Log.Error(ex.ToString());
        }
    }

}
