using DryIoc.Microsoft.DependencyInjection;

namespace Bootstrapper;

internal class BootstrapBuilder : IBootstrapBuilder
{
    private readonly IContainer _container;

    internal BootstrapBuilder(IContainer container)
    {
        _container = container;
        _container.Use<IServiceScopeFactory>(r => new DryIocServiceScopeFactory(r));
    }
        
    //tODO: command line parser?
    //todo: App configuration (defination file, persitent file name)

    public IBootstrapBuilder WithModulesFromAssembly<T>() where T : IAssemblyMarker
    {
        var modules = typeof(T).Assembly.GetTypes()
            .Where(type => typeof(IModule).IsAssignableFrom(type) && type.IsPublic && type.IsClass &&
                           !type.IsAbstract);

        foreach (var module in modules)
        {
            _container.Register(typeof(IModule), module);
        }

        return this;
    }

    public IBootstrapBuilder WithModule<T>() where T : IModule
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

    public void Run(Func<IResolverContext, Task> executionAction)
    {
        Parallel.ForEach(_container.ResolveMany<IModule>(), module =>
        {
            var serviceCollection = new ServiceCollection();
            module.Load(_container, serviceCollection);
            _container.Populate(serviceCollection);
        });
        
        using var scope = _container.OpenScope();
        executionAction(scope);
    }
}