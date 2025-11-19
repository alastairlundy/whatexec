using System.Threading.Tasks;

namespace AlastairLundy.WhatExec.Cli.Commands.SingleSearch;

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

    public class Settings : SingleSearchBaseCommandSettings
    {
        public override ValidationResult Validate()
        {
            if (DisableInteractivity)
            {
                if (Commands is null)
                    return ValidationResult.Error(Resources.ValidationErrors_File_NotSpecified);

                if (string.IsNullOrWhiteSpace(Commands) || string.IsNullOrEmpty(Commands))
                    return ValidationResult.Error(
                        Resources.ValidationErrors_File_EmptyOrWhitespace
                    );
            }
            else
            {
                Commands ??= UserInputHelper.GetFileInput();
            }

            return base.Validate();
        }
    }

    protected override async Task<int> ExecuteAsync(
        CommandContext context,
        Settings settings,
        CancellationToken cancellationToken
    )
    {
        Task task = new Task(() =>
        {
            if (settings.UseCaching) { }
        });
    }
}
