using System;
using GarageGroup.Infra;

namespace Microsoft.Extensions.DependencyInjection;

public static class SocketsHttpHandlerProviderServiceCollectionExtensions
{
    public static IServiceCollection AddSocketsHttpHandlerProviderAsSingleton(this IServiceCollection services)
        =>
        InternalAddSocketsHttpHandlerProviderAsSingleton(
            services ?? throw new ArgumentNullException(nameof(services)));

    internal static IServiceCollection InternalAddSocketsHttpHandlerProviderAsSingleton(this IServiceCollection services)
        =>
        services.AddSingleton<ISocketsHttpHandlerProvider, DefaultSocketsHttpHandlerProvider>();
}
