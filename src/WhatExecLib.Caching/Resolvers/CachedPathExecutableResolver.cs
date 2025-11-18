/*
    WhatExecLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.IO;
using AlastairLundy.DotPrimitives.IO.Paths;
using AlastairLundy.WhatExecLib;
using AlastairLundy.WhatExecLib.Abstractions.Detectors;
using Microsoft.Extensions.Caching.Memory;

namespace WhatExecLib.Caching.Resolvers;

/// <summary>
///
/// </summary>
public class CachedPathExecutableResolver : PathExecutableResolver, ICachedPathExecutableResolver
{
    private readonly IMemoryCache _cache;

    private const string PathExtensionCacheName = "PathExtensionCacheData";
    private const string PathCacheName = "PathCacheData";

    private TimeSpan DefaultPathCacheLifespan { get; set; } = TimeSpan.FromMinutes(5);
    private TimeSpan DefaultPathExtensionsCacheLifespan { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    ///
    /// </summary>
    /// <param name="executableFileDetector"></param>
    /// <param name="cache"></param>
    public CachedPathExecutableResolver(
        IExecutableFileDetector executableFileDetector,
        IMemoryCache cache
    )
        : base(executableFileDetector)
    {
        _cache = cache;
    }

    protected string[]? GetPathContents()
    {
        string[]? pathContents = _cache.Get<string[]>(PathCacheName);

        if (pathContents is null)
        {
            pathContents = PathVariable.GetContents();

            _cache.Set(PathCacheName, pathContents, DefaultPathCacheLifespan);
        }

        return pathContents;
    }

    protected string[] GetPathExtensions()
    {
        string[]? pathContentsExtensions = _cache.Get<string[]>(PathExtensionCacheName);

        if (pathContentsExtensions is null)
        {
            pathContentsExtensions = PathVariable.GetPathFileExtensions();

            _cache.Set(
                PathExtensionCacheName,
                pathContentsExtensions,
                DefaultPathExtensionsCacheLifespan
            );
        }

        return pathContentsExtensions;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="inputFilePath"></param>
    /// <param name="pathExtensionsCacheLifetime"></param>
    /// <param name="pathCacheLifetime"></param>
    /// <returns></returns>
    public FileInfo ResolvePathEnvironmentExecutableFile(
        string inputFilePath,
        TimeSpan? pathExtensionsCacheLifetime,
        TimeSpan? pathCacheLifetime
    )
    {
        if (pathCacheLifetime is not null)
            DefaultPathCacheLifespan = pathCacheLifetime.Value;

        if (pathExtensionsCacheLifetime is not null)
            DefaultPathExtensionsCacheLifespan = pathExtensionsCacheLifetime.Value;

        return ResolvePathEnvironmentExecutableFile(inputFilePath);
    }
}
