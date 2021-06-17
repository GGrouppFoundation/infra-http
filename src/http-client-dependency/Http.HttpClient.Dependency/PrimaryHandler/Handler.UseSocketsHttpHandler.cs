#nullable enable

using System;
using System.Net.Http;
using PrimeFuncPack;

namespace GGroupp.Infra
{
    partial class PrimaryHandler
    {
        public static Dependency<SocketsHttpHandler> UseSocketsHttpHandler(
            this Dependency<ISocketsHttpHandlerProvider, ISocketsHttpHandlerConfiguration> dependency)
            =>
            InternalUseSocketsHttpHandler(
                dependency ?? throw new ArgumentNullException(nameof(dependency)));

        public static Dependency<SocketsHttpHandler> UseSocketsHttpHandler(
            Func<IServiceProvider, ISocketsHttpHandlerConfiguration> configurationResolver)
            =>
            InternalUseSocketsHttpHandler(
                configurationResolver ?? throw new ArgumentNullException(nameof(configurationResolver)));

        public static Dependency<SocketsHttpHandler> UseStandardSocketsHttpHandler()
            =>
            InternalUseSocketsHttpHandler(
                sp => sp.GetSocketsHttpHandlerConfigurationFromEnvironment(string.Empty));

        private static Dependency<SocketsHttpHandler> InternalUseSocketsHttpHandler(
            Dependency<ISocketsHttpHandlerProvider, ISocketsHttpHandlerConfiguration> dependency)
            =>
            dependency.Fold(
                (provider, config) => provider.GetOrCreate(config.Name, handler => ConfigureSocketsHandler(handler, config)));

        private static Dependency<SocketsHttpHandler> InternalUseSocketsHttpHandler(
            Func<IServiceProvider, ISocketsHttpHandlerConfiguration> configurationResolver)
            =>
            Dependency.Create(
                sp => sp.GetServiceOrThrow<ISocketsHttpHandlerProvider>())
            .With(
                configurationResolver)
            .Pipe(
                InternalUseSocketsHttpHandler);
        
        private static void ConfigureSocketsHandler(SocketsHttpHandler httpHandler, ISocketsHttpHandlerConfiguration? configuration)
        {
            var pooledConnectionIdleTimeout = configuration?.PooledConnectionIdleTimeout;
            if(pooledConnectionIdleTimeout.HasValue)
            {
                httpHandler.PooledConnectionIdleTimeout = pooledConnectionIdleTimeout.Value;
            }

            var pooledConnectionLifetime = configuration?.PooledConnectionLifetime;
            if(pooledConnectionLifetime.HasValue)
            {
                httpHandler.PooledConnectionLifetime = pooledConnectionLifetime.Value;
            }

            var maxConnectionsPerServer = configuration?.MaxConnectionsPerServer;
            if(maxConnectionsPerServer.HasValue)
            {
                httpHandler.MaxConnectionsPerServer = maxConnectionsPerServer.Value;
            }
        }
    }
}