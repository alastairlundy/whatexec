using System.Threading;
using System.Threading.Tasks;
using AlastairLundy.WhatExec.Cli.Helpers;
using AlastairLundy.WhatExec.Cli.Localizations;
using AlastairLundy.WhatExec.Cli.Localizations;
using AlastairLundy.WhatExec.Cli.Settings;
using AlastairLundy.WhatExecLib.Abstractions;
using Spectre.Console;
using Spectre.Console.Cli;
using WhatExecLib.Caching;

namespace AlastairLundy.WhatExec.Cli.Commands;

public class PathOnlySearchCommand : AsyncCommand<PathOnlySearchCommand.Settings>
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

    public class Settings : WhatExecBaseCommandSettings
    {
        [CommandArgument(0, "<File or Command>")]
        public string? File { get; set; }

        public override ValidationResult Validate()
        {
            if (DisableInteractivity)
            {
                if (File is null)
                    return ValidationResult.Error(Resources.ValidationErrors_File_NotSpecified);

                if (string.IsNullOrWhiteSpace(File) || string.IsNullOrEmpty(File))
                    return ValidationResult.Error(
                        Resources.ValidationErrors_File_EmptyOrWhitespace
                    );
            }
            else
            {
                File ??= UserInputHelper.GetFileInput();
            }

            return base.Validate();
        }
    }

    public override async Task<int> ExecuteAsync(
        CommandContext context,
        Settings settings,
        CancellationToken cancellationToken
    )
    {
        AnsiConsole.Progress()
            .AutoRefresh(false)
            .
    }
}
