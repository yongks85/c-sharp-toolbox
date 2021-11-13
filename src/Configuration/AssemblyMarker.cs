using System.Runtime.CompilerServices;
using Bootstrapper.Abstractions;
using DryIoc;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Configuration.Tests")]

namespace Configuration;

// ReSharper disable once UnusedType.Global
public interface AssemblyMarker :IAssemblyMarker { }

public class Registration : IModule
{
    public void Load(IRegistrator builder, IServiceCollection services)
    {
        builder.Register<Config>();
        
        builder.Register<IPersistenceSetting, PersistInMemory>();
        builder.Register<IPersistenceSetting, PersistInFile>();
    }
}