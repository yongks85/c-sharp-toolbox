using System.Reflection;

namespace Utilities;

/// <summary>
/// Extract Information from Assembly
/// </summary>
public class AssemblyInfoExtractor
{
    public Version Version { get; }

    public DateTime BuildDate { get; }

    public string Path { get; }

    public string Name { get; }

    public AssemblyInfoExtractor(Assembly assembly)
    {
        Version = assembly.GetName().Version;
        BuildDate = File.GetLastWriteTime(assembly.Location);
        Path = System.IO.Path.Combine(assembly.Location);
        Name = assembly.GetName().Name;
    }

    public override string ToString() => $"{Name}, {Version}, {BuildDate} at {Path}";
}