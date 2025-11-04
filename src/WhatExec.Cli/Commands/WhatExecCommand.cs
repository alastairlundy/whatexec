using System.Threading;
using AlastairLundy.WhatExec.Cli.Settings;
using Spectre.Console.Cli;

namespace AlastairLundy.WhatExec.Cli.Commands;

public class WhatExecCommand : Command<WhatExecCommandSettings>
{
    public override int Execute(
        CommandContext context,
        WhatExecCommandSettings settings,
        CancellationToken cancellationToken
    )
    {
        throw new System.NotImplementedException();
    }
}
