using System;
using System.Globalization;
using AlastairLundy.WhatExec.Cli.Commands;
using AlastairLundy.WhatExecLib;
using AlastairLundy.WhatExecLib.Abstractions;
using AlastairLundy.WhatExecLib.Detectors;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Extensions.DependencyInjection;

IServiceCollection services = new ServiceCollection();

services.AddScoped<IExecutableFileDetector, ExecutableFileDetector>();
services.AddScoped<IExecutableFileInstancesLocator, IExecutableFileInstancesLocator>();
services.AddScoped<IMultiExecutableLocator, MultiExecutableLocator>();

using var registrar = new DependencyInjectionRegistrar(services);
CommandApp app = new CommandApp(registrar);

app.Configure(config =>
{
    config.CaseSensitivity(CaseSensitivity.Commands);
    config.SetApplicationCulture(CultureInfo.CurrentCulture);
    config.SetApplicationName("xpwhich");
    config.UseAssemblyInformationalVersion();

    Array.ForEach(args, x =>
    {
        if (x.ToLower().Contains("pretty"))
            app.SetDefaultCommand<PrettyWhatExecCommand>();
        else
        {
            app.SetDefaultCommand<WhatExecCommand>();
        }
    });

    config.AddCommand<PrettyWhatExecCommand>("pretty");
    
    config.AddCommand<WhichCompatCommand>("posix")
        .WithAlias("nix");
});


return app.Run(args);