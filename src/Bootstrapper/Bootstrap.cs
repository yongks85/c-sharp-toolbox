namespace Bootstrapper;

/// <summary>
/// Entry point to start using Bootstrapper
/// </summary>
public static class Bootstrap
{
    /// <summary>
    /// Instantiate and start a new configuration
    /// </summary>
    public static IBootstrapBuilder Using(IContainer container) => new BootstrapBuilder(container);

}