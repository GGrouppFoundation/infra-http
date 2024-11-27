using System;
using System.Net.Http;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using PrimeFuncPack;

namespace GarageGroup.Infra;

public static partial class PrimaryHandler
{
    private static readonly Lazy<DefaultSocketsHttpHandlerProvider> defaultSocketsHttpHandlerProvider;

    static PrimaryHandler()
    {
        defaultSocketsHttpHandlerProvider = new(CreateProvider, LazyThreadSafetyMode.ExecutionAndPublication);

        static DefaultSocketsHttpHandlerProvider CreateProvider()
            =>
            new();
    }

    private static Dependency<SocketsHttpHandler> InnerUseSocketsHttpHandler(
        Func<IServiceProvider, ISocketsHttpHandlerConfiguration> configurationResolver)
    {
        return Dependency.From(ResolveHandler);

        SocketsHttpHandler ResolveHandler(IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);

            var provider = serviceProvider.GetService<ISocketsHttpHandlerProvider>() ?? defaultSocketsHttpHandlerProvider.Value;
            var configuration = configurationResolver.Invoke(serviceProvider);

            return InnerCreateSocketsHttpHandler(provider, configuration);
        }
    }

    private static SocketsHttpHandler InnerCreateSocketsHttpHandler(
        ISocketsHttpHandlerProvider provider, ISocketsHttpHandlerConfiguration configuration)
    {
        return provider.GetOrCreate(configuration.Name, InnerConfigureSocketsHandler);

        void InnerConfigureSocketsHandler(SocketsHttpHandler httpHandler)
        {
            var pooledConnectionIdleTimeout = configuration?.PooledConnectionIdleTimeout;
            if (pooledConnectionIdleTimeout.HasValue)
            {
                httpHandler.PooledConnectionIdleTimeout = pooledConnectionIdleTimeout.Value;
            }

            var pooledConnectionLifetime = configuration?.PooledConnectionLifetime;
            if (pooledConnectionLifetime.HasValue)
            {
                httpHandler.PooledConnectionLifetime = pooledConnectionLifetime.Value;
            }

            var maxConnectionsPerServer = configuration?.MaxConnectionsPerServer;
            if (maxConnectionsPerServer.HasValue)
            {
                httpHandler.MaxConnectionsPerServer = maxConnectionsPerServer.Value;
            }
        }
    }
}