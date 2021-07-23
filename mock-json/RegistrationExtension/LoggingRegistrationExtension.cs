using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace mock_json.RegistrationExtension
{
    public static class LoggingRegistrationExtension
    {
        public static IServiceCollection AddLogger(this IServiceCollection services)
            => services.AddSingleton<ILogger>(opt =>
            {
                return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo
                .Console(theme: SystemConsoleTheme.Literate)
                .CreateLogger();
            });
    }
}