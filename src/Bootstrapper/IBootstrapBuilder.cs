namespace Bootstrapper;

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
    /// Scan the assembly of <see cref="IAssemblyMarker"/> for <see cref="IModule"/> to register
    /// </summary>
    IBootstrapBuilder WithModulesFromAssembly<T>() where T : IAssemblyMarker;

    /// <summary>
    /// For loose or custom registration
    /// </summary>
    IBootstrapBuilder WithLooseRegistration(Action<IRegistrator> registration);

    /// <summary>
    /// Run Application
    /// </summary>
    /// <param name="executionAction">Entry point of application </param>
    void Run(Func<IResolverContext, Task> executionAction);

    /// <summary>
    /// Add Unhandled Exception Implementation
    /// </summary>
    IBootstrapBuilder WithAppLevelExceptionHandling(Action<object, UnhandledExceptionEventArgs> currentDomainOnUnhandledException);

}