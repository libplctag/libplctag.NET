﻿// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using libplctag;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpDotNetCore
{
    class ExampleILogger
    {
        public static void Run()
        {
            Host.CreateDefaultBuilder(null)
                .ConfigureServices(services =>
                {
                    services.AddLibPlcTagLogging();
                    services.AddHostedService<Example>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                    logging.SetMinimumLevel(LogLevel.Debug);
                })
                .Build()
                .Run();
        }

        class Example : IHostedService
        {
            public async Task StartAsync(CancellationToken cancellationToken)
            {
                var myTag = new Tag()
                {
                    Name = "MyTag[0]",
                    Gateway = "127.0.0.1",
                    Path = "1,0",
                    PlcType = PlcType.ControlLogix,
                    Protocol = Protocol.ab_eip,
                    Timeout = TimeSpan.FromSeconds(5),
                };

                await myTag.InitializeAsync();
            }

            public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        }
    }

    class LibPlcTagLogSource : IHostedService
    {
        private readonly ILogger<LibPlcTagLogSource> _logger;

        public LibPlcTagLogSource(ILogger<LibPlcTagLogSource> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            LibPlcTag.LogEvent += LibPlcTag_LogEvent;
            LibPlcTag.DebugLevel = DebugLevel.Spew;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            LibPlcTag.LogEvent -= LibPlcTag_LogEvent;
            return Task.CompletedTask;
        }

        private void LibPlcTag_LogEvent(object sender, LogEventArgs e)
        {
            var logLevel = e.DebugLevel switch
            {
                DebugLevel.None     => LogLevel.None,
                DebugLevel.Error    => LogLevel.Error,
                DebugLevel.Warn     => LogLevel.Warning,
                DebugLevel.Info     => LogLevel.Information,
                DebugLevel.Detail   => LogLevel.Debug,
                DebugLevel.Spew     => LogLevel.Trace,
                _                   => throw new NotImplementedException(),
            };
            _logger.Log(logLevel, e.Message);
        }
    }

    static class LibPlcTagLoggingExtensions
    {
        public static IServiceCollection AddLibPlcTagLogging(this IServiceCollection services)
        {
            services.AddHostedService<LibPlcTagLogSource>();
            return services;
        }
    }
}
