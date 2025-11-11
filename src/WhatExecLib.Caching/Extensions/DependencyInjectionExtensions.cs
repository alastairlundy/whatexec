/*
    WhatExecLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

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
                services.AddScoped<ICachedPathExecutableResolver, CachedPathExecutableResolver>();
                break;
            case ServiceLifetime.Singleton:
                services.AddSingleton<
                    ICachedPathExecutableResolver,
                    CachedPathExecutableResolver
                >();
                break;
            case ServiceLifetime.Transient:
                services.AddTransient<
                    ICachedPathExecutableResolver,
                    CachedPathExecutableResolver
                >();
                break;
        }

        return services;
    }
}
