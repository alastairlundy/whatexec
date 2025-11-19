namespace AlastairLundy.WhatExec.Cli.Commands.SingleSearch;

public class DriveOnlySearchCommand : Command<DriveOnlySearchCommand.Settings>
{
    private readonly IWhatExecutableResolver _whatExecutableResolver;
    private readonly ICachedPathExecutableResolver _cachedPathExecutableResolver;

    public DriveOnlySearchCommand(
        IWhatExecutableResolver whatExecutableResolver,
        ICachedPathExecutableResolver cachedPathExecutableResolver
    )
    {
        _whatExecutableResolver = whatExecutableResolver;
        _cachedPathExecutableResolver = cachedPathExecutableResolver;
    }

    public class Settings : SingleSearchBaseCommandSettings
    {
        [CommandOption("-d|--drive")]
        public string? Drive { get; set; }

        [CommandOption("-f|--file")]
        public string? File { get; set; }

        public override ValidationResult Validate()
        {
            if (DisableInteractivity)
            {
                if (Drive is null)
                    return ValidationResult.Error(Resources.ValidationErrors_Drive_NotSpecified);

                if (File is null)
                    return ValidationResult.Error(Resources.ValidationErrors_File_NotSpecified);

                if (string.IsNullOrWhiteSpace(File) || string.IsNullOrEmpty(File))
                    return ValidationResult.Error(
                        Resources.ValidationErrors_File_EmptyOrWhitespace
                    );
            }
            else
            {
                Drive ??= UserInputHelper.GetDriveInput();
                File ??= UserInputHelper.GetFileInput();
            }

            return base.Validate();
        }
    }

    protected override int Execute(
        CommandContext context,
        Settings settings,
        CancellationToken cancellationToken
    )
    {
        throw new System.NotImplementedException();
    }
}
