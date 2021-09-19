using System;
using System.Threading.Tasks;
using DryIoc;

namespace Bootstrapper
{
    /// <summary>
    /// Application Bootstrapper Builder
    /// </summary>
    public interface IBootstrapBuilder
    {
        /// <summary>
        /// Register features to Container
        /// </summary>
        IBootstrapBuilder WithModule<T>() where T : IModule;

        /// <summary>
        /// Register modules using Microsoft Dependency Injection extensions
        /// </summary>
        IBootstrapBuilder WithMicrosoftDependencyRegistration(Action<ServiceCollection> registerMsDi);

        /// <summary>
        /// For loose or custom registration
        /// </summary>
        IBootstrapBuilder WithLooseRegistration(Action<IRegistrator> registration);

        /// <summary>
        /// Run Application
        /// </summary>
        /// <param name="executionAction">Entry point of application </param>
        void Run(Func<IResolver,Task> executionAction);

        /// <summary>
        /// Add Unhandled Exception Implementation
        /// </summary>
        IBootstrapBuilder WithAppLevelExceptionHandling(Action<object, UnhandledExceptionEventArgs> currentDomainOnUnhandledException);

    }
}