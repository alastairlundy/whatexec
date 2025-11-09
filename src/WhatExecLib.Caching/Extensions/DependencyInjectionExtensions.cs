using System;
using AlastairLundy.WhatExecLib.Abstractions;
using AlastairLundy.WhatExecLib.Abstractions.Detectors;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using WhatExecLib.Caching.Resolvers;

namespace WhatExecLib.Caching.Extensions;

/// <summary>
///
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="services"></param>
    /// <param name="serviceLifetime"></param>
    /// <param name="pathCacheLifespan"></param>
    /// <param name="pathExtensionsCacheLifespan"></param>
    /// <returns></returns>
    public static IServiceCollection UseCachingWithWhatExecLib(
        this IServiceCollection services,
        ServiceLifetime serviceLifetime,
        TimeSpan? pathCacheLifespan = null,
        TimeSpan? pathExtensionsCacheLifespan = null
    )
    {
        switch (serviceLifetime)
        {
            case ServiceLifetime.Scoped:
                services.AddScoped<IPathExecutableResolver>(sp => new CachedPathExecutableResolver(
                    sp.GetRequiredService<IExecutableFileDetector>(),
                    sp.GetRequiredService<IMemoryCache>(),
                    pathCacheLifespan,
                    pathExtensionsCacheLifespan
                ));
                break;
            case ServiceLifetime.Singleton:
                services.AddSingleton<IPathExecutableResolver>(
                    sp => new CachedPathExecutableResolver(
                        sp.GetRequiredService<IExecutableFileDetector>(),
                        sp.GetRequiredService<IMemoryCache>(),
                        pathCacheLifespan,
                        pathExtensionsCacheLifespan
                    )
                );
                break;
            case ServiceLifetime.Transient:
                services.AddTransient<IPathExecutableResolver>(
                    sp => new CachedPathExecutableResolver(
                        sp.GetRequiredService<IExecutableFileDetector>(),
                        sp.GetRequiredService<IMemoryCache>(),
                        pathCacheLifespan,
                        pathExtensionsCacheLifespan
                    )
                );
                break;
        }

        return services;
    }
}
