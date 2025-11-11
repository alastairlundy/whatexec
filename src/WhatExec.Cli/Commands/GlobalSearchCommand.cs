using System.Threading;
using AlastairLundy.WhatExec.Cli.Helpers;
using AlastairLundy.WhatExec.Cli.Localizations;
using AlastairLundy.WhatExec.Cli.Settings;
using AlastairLundy.WhatExecLib.Abstractions;
using Spectre.Console;
using Spectre.Console.Cli;
using WhatExecLib.Caching;

namespace AlastairLundy.WhatExec.Cli.Commands;

public class GlobalSearchCommand : Command<GlobalSearchCommand.Settings>
{
    private readonly IWhatExecutableResolver _whatExecutableResolver;
    private readonly ICachedPathExecutableResolver _cachedPathExecutableResolver;

    public GlobalSearchCommand(
        IWhatExecutableResolver whatExecutableResolver,
        ICachedPathExecutableResolver cachedPathExecutableResolver
    )
    {
        _whatExecutableResolver = whatExecutableResolver;
        _cachedPathExecutableResolver = cachedPathExecutableResolver;
    }

    public class Settings : WhatExecBaseCommandSettings
    {
        [CommandOption("-f|--file")]
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

    public override int Execute(
        CommandContext context,
        Settings settings,
        CancellationToken cancellationToken
    ) { }
}
