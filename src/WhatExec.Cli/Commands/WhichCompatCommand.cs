using System.Threading;
using AlastairLundy.WhatExec.Cli.Settings;
using Spectre.Console.Cli;

namespace AlastairLundy.WhatExec.Cli.Commands;

public class WhichCompatCommand : Command<WhichCompatCommandSettings>
{
    public override int Execute(
        CommandContext context,
        WhichCompatCommandSettings settings,
        CancellationToken cancellationToken
    )
    {
        throw new System.NotImplementedException();
    }
}
