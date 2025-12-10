/*
    WhatExecLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using AlastairLundy.DotExtensions.IO.Permissions;
using AlastairLundy.DotPrimitives.IO.Paths;

// ReSharper disable ConvertClosureToMethodGroup

namespace WhatExecLib;

/// <summary>
/// Provides functionality to resolve the path of an executable file based on the system's PATH environment variable.
/// </summary>
public class PathExecutableResolver : IPathExecutableResolver
{
    private readonly IExecutableFileDetector _executableFileDetector;
    protected bool IsUnix { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="executableFileDetector"></param>
    public PathExecutableResolver(IExecutableFileDetector executableFileDetector)
    {
        _executableFileDetector = executableFileDetector;
        IsUnix =
            OperatingSystem.IsLinux()
            || OperatingSystem.IsMacOS()
            || OperatingSystem.IsMacCatalyst()
            || OperatingSystem.IsFreeBSD()
            || OperatingSystem.IsAndroid();
    }

    #region Helper Methods
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("freebsd")]
    [SupportedOSPlatform("android")]
    protected bool ExecutableFileIsValid(string filePath, out FileInfo? fileInfo)
    {
        try
        {
            string fullFilePath = Path.GetFullPath(filePath);

            FileInfo file = new FileInfo(fullFilePath);

            if (IsUnix)
            {
                if (file.HasExecutePermission())
                {
                    fileInfo = file;
                    return true;
                }
            }
            else
            {
                if (File.Exists(fullFilePath))
                {
                    fileInfo = file;
                    return true;
                }
            }

            fileInfo = null;
            return false;
        }
        catch (ArgumentException)
        {
            fileInfo = null;
            return false;
        }
    }

    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("freebsd")]
    [SupportedOSPlatform("android")]
    protected bool CheckFileExists(
        string inputFilePath,
        out FileInfo? fileInfo,
        string pathEntry,
        string pathExtension
    )
    {
        string filePath = Path.Combine(pathEntry, $"{inputFilePath}{pathExtension}");

        if (File.Exists(filePath))
        {
            if (ExecutableFileIsValid(filePath, out FileInfo? info) && info is not null)
            {
                fileInfo = info;
                return File.Exists(Path.GetFullPath(filePath));
            }
        }

        fileInfo = null;
        return false;
    }
    #endregion

    /// <summary>
    /// Resolves a file from the system's PATH environment variable using the provided file name.
    /// </summary>
    /// <param name="inputFilePath">The name of the file to resolve, including optional relative or absolute paths.</param>
    /// <returns>A <see cref="FileInfo"/> object representing the resolved file.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file could not be found in the specified locations.</exception>
    /// <exception cref="PlatformNotSupportedException">Thrown if the current platform is unsupported.</exception>
    /// <exception cref="InvalidOperationException">Thrown if an invalid operation occurs during file resolution, such as PATH not being able to be resolved.</exception>
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("freebsd")]
    [SupportedOSPlatform("android")]
    public FileInfo ResolveExecutableFile(string inputFilePath) =>
        ResolveExecutableFiles(inputFilePath).First();

    /// <summary>
    ///
    /// </summary>
    /// <param name="inputFilePaths"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("freebsd")]
    [SupportedOSPlatform("android")]
    public FileInfo[] ResolveExecutableFiles(params string[] inputFilePaths)
    {
        ArgumentNullException.ThrowIfNull(inputFilePaths);

        string[] pathExtensions = PathEnvironmentVariable.GetPathFileExtensions();
        string[] pathContents =
            PathEnvironmentVariable.GetDirectories()
            ?? throw new InvalidOperationException("PATH Variable could not be found.");

        bool foundAny = false;
        List<FileInfo> output = new();

        foreach (string inputFilePath in inputFilePaths)
        {
            if (
                Path.IsPathRooted(inputFilePath)
                || inputFilePath.Contains(Path.DirectorySeparatorChar)
                || inputFilePath.Contains(Path.AltDirectorySeparatorChar)
            )
            {
                if (File.Exists(inputFilePath))
                {
                    if (
                        ExecutableFileIsValid(inputFilePath, out FileInfo? info) && info is not null
                    )
                    {
                        output.Add(info);
                        foundAny = true;
                    }
                }
            }

            bool fileHasExtension = Path.GetExtension(inputFilePath) != string.Empty;

            foreach (string pathEntry in pathContents)
            {
                if (!fileHasExtension)
                {
                    pathExtensions = [""];
                }

                foreach (string pathExtension in pathExtensions)
                {
                    bool result = CheckFileExists(
                        inputFilePath,
                        out FileInfo? fileInfo,
                        pathEntry,
                        pathExtension
                    );

                    if (result && fileInfo is not null)
                    {
                        foundAny = true;
                        output.Add(fileInfo);
                    }
                }
            }
        }

        return foundAny
            ? output.ToArray()
            : throw new FileNotFoundException(
                $"Could not find file(s): {string.Join(",", inputFilePaths)}"
            );
    }

    /// <summary>
    /// Attempts to resolve a file from the system's PATH environment variable using the provided file name.
    /// </summary>
    /// <param name="inputFilePath">The name of the file to resolve, including optional relative or absolute paths.</param>
    /// <param name="fileInfo">When this method returns, contains the resolved <see cref="FileInfo"/> object if the resolution is successful; otherwise, null.</param>
    /// <returns>True if the file is successfully resolved; otherwise, false.</returns>
    /// <exception cref="PlatformNotSupportedException">Thrown if the current platform is unsupported.</exception>
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("freebsd")]
    [SupportedOSPlatform("android")]
    public bool TryResolveExecutableFile(string inputFilePath, out FileInfo? fileInfo)
    {
        bool success = TryResolveExecutableFiles([inputFilePath], out FileInfo[]? fileInfos);

        fileInfo = fileInfos?.FirstOrDefault() ?? null;

        return success;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="inputFilePaths"></param>
    /// <param name="fileInfos"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("freebsd")]
    [SupportedOSPlatform("android")]
    public bool TryResolveExecutableFiles(string[] inputFilePaths, out FileInfo[]? fileInfos)
    {
        ArgumentNullException.ThrowIfNull(inputFilePaths);

        string[] pathExtensions = PathEnvironmentVariable.GetPathFileExtensions();
        string[] pathContents;

        try
        {
            pathContents =
                PathEnvironmentVariable.GetDirectories()
                ?? throw new InvalidOperationException("PATH Variable could not be found.");
        }
        catch (InvalidOperationException)
        {
            fileInfos = null;
            return false;
        }

        bool foundAny = false;
        List<FileInfo> output = new();

        foreach (string inputFilePath in inputFilePaths)
        {
            if (
                Path.IsPathRooted(inputFilePath)
                || inputFilePath.Contains(Path.DirectorySeparatorChar)
                || inputFilePath.Contains(Path.AltDirectorySeparatorChar)
            )
            {
                if (File.Exists(inputFilePath))
                {
                    if (
                        ExecutableFileIsValid(inputFilePath, out FileInfo? info) && info is not null
                    )
                    {
                        output.Add(info);
                        foundAny = true;
                    }
                }
            }

            bool fileHasExtension = Path.GetExtension(inputFilePath) != string.Empty;

            foreach (string pathEntry in pathContents)
            {
                if (!fileHasExtension)
                {
                    pathExtensions = [""];
                }

                foreach (string pathExtension in pathExtensions)
                {
                    bool result = CheckFileExists(
                        inputFilePath,
                        out FileInfo? fileInfo,
                        pathEntry,
                        pathExtension
                    );

                    if (result && fileInfo is not null)
                    {
                        foundAny = true;
                        output.Add(fileInfo);
                    }
                }
            }
        }

        if (foundAny)
        {
            fileInfos = output.ToArray();
            return true;
        }

        fileInfos = null;
        return false;
    }
}
