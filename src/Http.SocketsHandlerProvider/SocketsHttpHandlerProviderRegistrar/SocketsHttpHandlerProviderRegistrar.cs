using System;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Hosting;

public static class SocketsHttpHandlerProviderRegistrar
{
    public static void RegisterSingleton(this IServiceCollection services)
        =>
        SocketsHttpHandlerProviderServiceCollectionExtensions.InternalAddSocketsHttpHandlerProviderAsSingleton(
                services ?? throw new ArgumentNullException(nameof(services)));
}

