namespace WhatExecLib.Extensions;

public static class PrioritizeLocationsExtensions
{
    private static int ComputeDirectoryPriorityScore(FileInfo fileInfo)
    {
        string dirPathName =
            (fileInfo.DirectoryName?.ToLower() ?? fileInfo.Directory?.Name)
            ?? fileInfo.FullName.Split(Path.DirectorySeparatorChar)[^1];

        if (dirPathName.StartsWith(Environment.GetFolderPath(Environment.SpecialFolder.Programs)))
            return 0;

        if (OperatingSystem.IsWindows())
        {
            if (
                dirPathName.StartsWith(Environment.GetFolderPath(Environment.SpecialFolder.Windows))
            )
                return 1;

            if (
                dirPathName.StartsWith(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                )
            )
                return 2;

            if (
                dirPathName.StartsWith(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
                )
            )
                return 2;

            if (
                dirPathName.StartsWith(
                    Environment.GetFolderPath(Environment.SpecialFolder.AdminTools)
                )
            )
                return 3;
        }

        if (dirPathName.StartsWith(Environment.GetFolderPath(Environment.SpecialFolder.System)))
            return 2;

        if (
            dirPathName.StartsWith(
                Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
            )
        )
            return 4;

        return 10;
    }

    internal static IEnumerable<FileInfo> PrioritizeLocations(this IEnumerable<FileInfo> files)
    {
        return files.OrderBy(x => ComputeDirectoryPriorityScore(x));
    }
}
