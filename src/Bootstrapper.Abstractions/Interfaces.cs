using Microsoft.Extensions.DependencyInjection;

namespace Bootstrapper.Abstractions;

/// <summary>
/// Assembly Marker used for assembly scanning
/// Interface is empty by default.
/// Technically any public type in the assembly can be used.
/// But by creating a empty type, this makes the code clearer
/// Each Project that require scanning should inherit this interface for IoC Assembly scanning
/// </summary>
public interface IAssemblyMarker { }

/// <summary>
/// Registration Module
/// </summary>
public interface IModule
{
    //Register Module
    void Load(IRegistrator registrator, IServiceCollection services);
}