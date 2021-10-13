using System;
using GGroupp.Infra;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SocketsHttpHandlerProviderServiceCollectionExtensions
    {
        public static IServiceCollection AddSocketsHttpHandlerProviderAsSingleton(this IServiceCollection services)
            =>
            InternalAddSocketsHttpHandlerProviderAsSingleton(
                services ?? throw new ArgumentNullException(nameof(services)));

        internal static IServiceCollection InternalAddSocketsHttpHandlerProviderAsSingleton(IServiceCollection services)
            =>
            services.AddSingleton<ISocketsHttpHandlerProvider, DefaultSocketsHttpHandlerProvider>();
    }
}

