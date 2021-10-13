using System;
using System.Net.Http;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using PrimeFuncPack;

namespace GGroupp.Infra;

public static partial class PrimaryHandler
{
    private static readonly Lazy<DefaultSocketsHttpHandlerProvider> defaultSocketsHttpHandlerProvider;

    static PrimaryHandler()
        =>
        defaultSocketsHttpHandlerProvider = new(() => new(), LazyThreadSafetyMode.ExecutionAndPublication);

    private static Dependency<SocketsHttpHandler> InnerUseSocketsHttpHandler(
        Func<IServiceProvider, ISocketsHttpHandlerConfiguration> configurationResolver)
        =>
        Dependency.From(
            sp => sp.GetService<ISocketsHttpHandlerProvider>() ?? defaultSocketsHttpHandlerProvider.Value)
        .With(
            configurationResolver)
        .Pipe(
            InnerUseSocketsHttpHandler);

    private static Dependency<SocketsHttpHandler> InnerUseSocketsHttpHandler(
        Dependency<ISocketsHttpHandlerProvider, ISocketsHttpHandlerConfiguration> dependency)
        =>
        dependency.Fold(
            (provider, config) => provider.GetOrCreate(config.Name, handler => InnerConfigureSocketsHandler(handler, config)));

    private static void InnerConfigureSocketsHandler(SocketsHttpHandler httpHandler, ISocketsHttpHandlerConfiguration? configuration)
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
