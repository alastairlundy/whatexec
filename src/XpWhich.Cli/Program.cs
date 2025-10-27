using System;
using System.Globalization;

using Spectre.Console.Cli;

using XpWhich.Cli.Commands;

CommandApp app = new CommandApp();

app.Configure(config =>
{
    config.CaseSensitivity(CaseSensitivity.Commands);
    config.SetApplicationCulture(CultureInfo.CurrentCulture);
    config.SetApplicationName("xpwhich");
    config.UseAssemblyInformationalVersion();

    Array.ForEach(args, x =>
    {
        if (x.ToLower().Contains("pretty"))
            app.SetDefaultCommand<PrettyXpWhichCommand>();
        else
        {
            app.SetDefaultCommand<XpWhichCommand>();
        }
    });

    config.AddCommand<PrettyXpWhichCommand>("pretty");
    
    config.AddCommand<WhichCompatCommand>("posix")
        .WithAlias("nix");
});


return app.Run(args);