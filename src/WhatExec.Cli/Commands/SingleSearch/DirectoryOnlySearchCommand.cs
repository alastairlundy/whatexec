namespace AlastairLundy.WhatExec.Cli.Commands.SingleSearch;

public class DirectoryOnlySearchCommand : Command<DirectoryOnlySearchCommand.Settings>
{
    private readonly IWhatExecutableResolver _whatExecutableResolver;
    private readonly ICachedPathExecutableResolver _cachedPathExecutableResolver;

    public DirectoryOnlySearchCommand(
        IWhatExecutableResolver whatExecutableResolver,
        ICachedPathExecutableResolver cachedPathExecutableResolver
    )
    {
        _whatExecutableResolver = whatExecutableResolver;
        _cachedPathExecutableResolver = cachedPathExecutableResolver;
    }

    public class Settings : SingleSearchBaseCommandSettings
    {
        [CommandOption("-d|--directory")]
        public string? Directory { get; set; }

        [CommandOption("-f|--file")]
        public string? File { get; set; }

        public override ValidationResult Validate()
        {
            if (DisableInteractivity)
            {
                if (Directory is null)
                    return ValidationResult.Error(
                        Resources.ValidationErrors_Directory_NotSpecified
                    );

                if (File is null)
                    return ValidationResult.Error(Resources.ValidationErrors_File_NotSpecified);

                if (string.IsNullOrWhiteSpace(File) || string.IsNullOrEmpty(File))
                    return ValidationResult.Error(
                        Resources.ValidationErrors_File_EmptyOrWhitespace
                    );
            }
            else
            {
                string drive = UserInputHelper.GetDriveInput();

                Directory ??= UserInputHelper.GetDirectoryInput(new DriveInfo(drive));
                File ??= UserInputHelper.GetFileInput();
            }

            return base.Validate();
        }
    }

    protected override int Execute(
        CommandContext context,
        Settings settings,
        CancellationToken cancellationToken
    ) { }
}
