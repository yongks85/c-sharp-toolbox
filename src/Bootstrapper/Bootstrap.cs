using System;
using System.Threading.Tasks;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Bootstrapper
{
    public class Bootstrap: IBootstrapBuilder
    {
        private readonly IContainer _container;

        /// <summary>
        /// Instantiate and start a new configuration
        /// </summary>
        public static IBootstrapBuilder Using(IContainer container) => new Bootstrap(container);


        private Bootstrap(IContainer container)
        {
            _container = container;
        }

        public IBootstrapBuilder WithMicrosoftDependencyRegistration(Action<ServiceCollection> registerMsDi)
        {
            _container.With(rules => rules.With(FactoryMethod.ConstructorWithResolvableArguments));
            _container.Use<IServiceScopeFactory>(r => new DryIocServiceScopeFactory(r));

            var serviceCollection = new ServiceCollection();
            registerMsDi(serviceCollection);
            _container.Populate(serviceCollection);

            return this;
        }

        public IBootstrapBuilder WithModule<T>() where T:IModule
        {
            _container.RegisterMany<T>();
            return this;
        }

        public IBootstrapBuilder WithLooseRegistration(Action<IRegistrator> registration)
        {
            registration(_container);
            return this;
        }

        public IBootstrapBuilder WithAppLevelExceptionHandling(Action<object, UnhandledExceptionEventArgs> currentDomainOnUnhandledException)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, arg) => currentDomainOnUnhandledException(sender, arg);
            return this;
        }

        public void Run(Func<IResolver, Task> executionAction)
        {
            Parallel.ForEach(_container.ResolveMany<IModule>(), module => module.Load(_container));
            executionAction(_container);
        }
    }
}
