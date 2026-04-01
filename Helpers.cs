using CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.ZipClasses;
namespace ProjectZipTool;
internal static partial class Helpers
{
    // Folders to skip
    private static readonly HashSet<string> _skipFolders =
    [
        "bin",
        "obj",
        ".vs",
        ".git",
        ".idea"
    ];

    public static async Task CreateZipFileAsync(string requestedRootLocation, string projectName, string projectLocation)
    {
        if (Directory.Exists(projectLocation) == false)
        {
            throw new DirectoryNotFoundException($"Project folder was not found: {projectLocation}");
        }

        string zipPath = Path.Combine(requestedRootLocation, $"{projectName}.zip");

        CustomZipClass zipClass = new();
        await ZipAsync(projectLocation, projectLocation, zipClass);
        await zipClass.SaveZipFileAsync(zipPath);
    }

    private static async Task ZipAsync(string rootFolder, string currentFolder, CustomZipClass zipClass)
    {
        // Add files in the current folder
        foreach (string file in Directory.GetFiles(currentFolder))
        {
            string? parentFolder = Path.GetDirectoryName(file);

            string relativeFolder = "";
            if (string.IsNullOrEmpty(parentFolder) == false)
            {
                relativeFolder = Path.GetRelativePath(rootFolder, parentFolder);

                // If file is directly in root, relative path becomes "."
                if (relativeFolder == ".")
                {
                    relativeFolder = "";
                }
            }

            if (string.IsNullOrWhiteSpace(relativeFolder))
            {
                zipClass.AddFileToZip(file);
            }
            else
            {
                zipClass.AddFileToZip(file, relativeFolder);
            }
        }

        // Recurse into subfolders
        foreach (string dir in Directory.GetDirectories(currentFolder))
        {
            string folderName = Path.GetFileName(dir);

            if (_skipFolders.Contains(folderName))
            {
                continue;
            }

            await ZipAsync(rootFolder, dir, zipClass);
        }
    }
}