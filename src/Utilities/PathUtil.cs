using System.Reflection;
using Ardalis.GuardClauses;

namespace Utilities;

public static class PathUtil
{
    public static void ReadEmbeddedFile(Assembly assembly, string fileWithNamespace, Action<StreamReader> readFile)
    {
        using var stream = assembly.GetManifestResourceStream(fileWithNamespace);
        using var reader = new StreamReader(stream);
        Guard.Against.Null(stream, nameof(fileWithNamespace));
        readFile(reader);
    }
}