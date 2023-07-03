using System;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting;

public static class SocketsHttpHandlerProviderHostBuilderExtensions
{
    public static IHostBuilder ConfigureSocketsHttpHandlerProvider(this IHostBuilder hostBuilder)
    {
        ArgumentNullException.ThrowIfNull(hostBuilder);

        return hostBuilder.ConfigureServices(ConfigureSocketsHttpHandlerProvider);

        static void ConfigureSocketsHttpHandlerProvider(IServiceCollection services)
            =>
            services.InternalAddSocketsHttpHandlerProviderAsSingleton();
    }
}