/*
    WhatExecLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using AlastairLundy.WhatExecLib.Caching.Extensions;
using AlastairLundy.WhatExecLib.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

IServiceCollection services = new ServiceCollection();

services.AddMemoryCache();
services.AddWhatExecLib(ServiceLifetime.Scoped);
services.AddWhatExecLibCaching(ServiceLifetime.Scoped);
