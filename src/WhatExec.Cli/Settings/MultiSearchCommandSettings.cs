namespace AlastairLundy.WhatExec.Cli.Settings;

public class MultiSearchCommandSettings : SingleSearchBaseCommandSettings
{
    [CommandOption("-a|--all")]
    [DefaultValue(false)]
    public bool PrintAllResults { get; set; }

    [CommandOption("-l|--limit")]
    [DefaultValue(3)]
    public int NumberOfResultsToShow { get; init; }

    public override ValidationResult Validate()
    {
        if (NumberOfResultsToShow < 0)
        {
            ValidationResult.Error(
                "Number of results to show must be greater than or equal to zero."
            );
        }

        if (UseCaching && CacheLifetimeMinutes is null)
            CacheLifetimeMinutes = 3.0;

        return base.Validate();
    }
}
