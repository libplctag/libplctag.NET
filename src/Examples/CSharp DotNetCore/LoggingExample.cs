using libplctag;
using libplctag.DataTypes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpDotNetCore
{
    class LoggingExample
    {
        public static async void Run()
        {
            LibPlcTag.Logger = new MyConsoleLogger();
            LibPlcTag.LogLevel = LogLevel.Information;
            var myTag = new Tag<DintPlcMapper, int>()
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

    }

    /// <summary>
    /// Ugh - how many times do I just need to create an instance of a logger but don't want to set up dependency injection!?
    /// </summary>
    class MyConsoleLogger : ILogger
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Console.WriteLine($"{logLevel} {state}");
        }
    }

    /// <summary>
    /// This would be used in the case where Dependency Injection is used.
    /// </summary>
    class LibPlcTagLogSource : IHostedService
    {
        private readonly ILogger<LibPlcTagLogSource> _logger;

        public LibPlcTagLogSource(ILogger<LibPlcTagLogSource> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            LibPlcTag.Logger = _logger;
            LibPlcTag.LogLevel = LogLevel.Trace;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
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
