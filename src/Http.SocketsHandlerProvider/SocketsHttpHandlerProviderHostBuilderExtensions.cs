using System;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting;

public static class SocketsHttpHandlerProviderHostBuilderExtensions
{
    public static IHostBuilder ConfigureSocketsHttpHandlerProvider(this IHostBuilder hostBuilder)
        =>
        InnerConfigureSocketsHttpHandlerProvider(
            hostBuilder ?? throw new ArgumentNullException(nameof(hostBuilder)));

    private static IHostBuilder InnerConfigureSocketsHttpHandlerProvider(IHostBuilder hostBuilder)
        =>
        hostBuilder.ConfigureServices(
            services => services.InternalAddSocketsHttpHandlerProviderAsSingleton());
}