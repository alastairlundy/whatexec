using System.Threading;
using AlastairLundy.WhatExec.Cli.Settings;
using Spectre.Console.Cli;

namespace AlastairLundy.WhatExec.Cli.Commands;

public class WhatExecCommand : Command<XpWhichCommandSettings>
{
    public override int Execute(CommandContext context, XpWhichCommandSettings settings,
        CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}