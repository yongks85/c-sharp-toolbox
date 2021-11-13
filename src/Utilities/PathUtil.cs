using System.Reflection;
using Ardalis.GuardClauses;

namespace Utilities;

public static class PathUtil
{
    /// <summary>
    /// Read the Embedded file
    /// </summary>
    /// <param name="assembly">Assembly</param>
    /// <param name="fileWithNamespace">file with the full assembly namespace</param>
    /// <param name="readFile">Callback to read the file</param>
    public static void ReadEmbeddedFile(Assembly assembly, string fileWithNamespace, Action<StreamReader> readFile)
    {
        using var stream = assembly.GetManifestResourceStream(fileWithNamespace);
        using var reader = new StreamReader(stream);
        Guard.Against.Null(stream, nameof(fileWithNamespace));
        readFile(reader);
    }

    /// <summary>
    /// Safely Delete Directories with guarding against Special Folders or root
    /// Will throw a exception if the delete is is a special folder.
    /// Help prevent loss when issue or carelessness occurs
    /// </summary>
    public static void GuardedDirectoryDelete(string path, bool recursive = false)
    {
        var specialFolders = Enum.GetValues(typeof(Environment.SpecialFolder))
            .Cast<Environment.SpecialFolder>().Select(Environment.GetFolderPath).ToList();

        if (specialFolders.Any(folder => folder == path))
        {
            throw new UnauthorizedAccessException("Delete of special folder detected");
        }

        if (recursive) { RecursiveDelete(new DirectoryInfo(path)); }
        else Directory.Delete(path);

        static void RecursiveDelete(DirectoryInfo baseDir)
        {
            if (!baseDir.Exists) return;

            foreach (var dir in baseDir.EnumerateDirectories())
            {
                RecursiveDelete(dir);
            }
            var files = baseDir.GetFiles();
            foreach (var file in files)
            {
                file.IsReadOnly = false;
                file.Delete();
            }
            baseDir.Delete();
        }
    }
}