using System.ComponentModel;
using Spectre.Console.Cli;

namespace AlastairLundy.WhatExec.Cli.Settings;

public class WhatExecCommandSettings : CommandSettings
{
    [CommandArgument(0, "<Command(s)>")]
    public string? Commands { get; init; }
    
    [CommandOption("--all|-a")]
    [DefaultValue(false)]
    public bool PrintAllResults { get; init; }
    
    [CommandOption("--limit|-l")]
    [DefaultValue(-1)]
    public int NumberOfResultsToShow { get; init; }
    
    [CommandOption("-s")]
    [DefaultValue(false)]
    public bool OnlyReturnExitCode { get; init; }
}