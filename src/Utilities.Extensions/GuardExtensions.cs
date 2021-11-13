using System.IO;
using Ardalis.GuardClauses;

namespace Utilities.Extensions;

public static class GuardExtensions
{

    /// <summary>
    /// Directory does not exist
    /// </summary>
    public static void DirectoryMissing(this IGuardClause guardClause, string input)
    {
        if (!Directory.Exists(input)) throw new DirectoryNotFoundException(input + " does not exist");
    }

    /// <summary>
    /// File does not Exists
    /// </summary>
    public static void FileMissing(this IGuardClause guardClause, string input)
    {
        if (!File.Exists(input)) throw new FileNotFoundException(input + " does not exist");
    }

    /// <summary>
    /// Path does not exists as file or directory
    /// </summary>
    public static void PathMissing(this IGuardClause guardClause, string input)
    {
        var attributes = File.GetAttributes(input);
        if (attributes == FileAttributes.Directory)
        {
            DirectoryMissing(guardClause, input);
        }
        else
        {
            FileMissing(guardClause, input);
        }
    }
}