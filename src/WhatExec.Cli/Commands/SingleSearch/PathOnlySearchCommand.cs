using System.Diagnostics;
using DotMake.CommandLine;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace AlastairLundy.WhatExec.Cli.Commands.SingleSearch;

public class PathOnlySearchCommand
{
    private readonly IPathExecutableResolver _pathExecutableResolver;
    private readonly ICachedPathExecutableResolver _cachedPathExecutableResolver;

    public PathOnlySearchCommand(
        IPathExecutableResolver pathExecutableResolver,
        ICachedPathExecutableResolver cachedPathExecutableResolver
    )
    {
        _pathExecutableResolver = pathExecutableResolver;
        _cachedPathExecutableResolver = cachedPathExecutableResolver;
    }

    [CliArgument(Name = "<Commands>", Description = "The commands to resolve the file path of.")]
    public string[]? Commands { get; set; }

    [CliOption(Name = "--use-caching")]
    [DefaultValue(true)]
    public bool UseCaching { get; set; }

    public async Task<int> RunAsync()
    {
        string[] commands = Commands ?? UserInputHelper.GetCommandInput();

        try
        {
            IList<FileInfo> ResolveCommands()
            {
                List<FileInfo> output = new List<FileInfo>();

                Stopwatch stopwatch = new();

                foreach (string command in commands)
                {
                    stopwatch.Start();
                    bool found;
                    FileInfo? info;

                    if (UseCaching)
                    {
                        found =
                            _cachedPathExecutableResolver.TryResolvePathEnvironmentExecutableFile(
                                command,
                                out info
                            );
                    }
                    else
                    {
                        found = _pathExecutableResolver.TryResolvePathEnvironmentExecutableFile(
                            command,
                            out info
                        );
                    }

                    if (found && info is not null)
                        output.Add(info);

                    stopwatch.Stop();
                    Console.WriteLine(
                        "Took {0}ms to resolve {1}",
                        stopwatch.Elapsed.TotalMilliseconds,
                        command
                    );
                    stopwatch.Reset();
                }

                return output;
            }

            Task<IList<FileInfo>> resolverTask = Task.Run(() => ResolveCommands());

            IList<FileInfo> resolvedCommands = await resolverTask;

            foreach (FileInfo resolvedCommand in resolvedCommands)
            {
                Console.WriteLine(resolvedCommand.FullName);
            }
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
            return 1;
        }

        return 0;
    }
}
