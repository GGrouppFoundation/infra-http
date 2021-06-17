#nullable enable

using System;

namespace GGroupp.Infra
{
    public interface ISocketsHttpHandlerConfiguration
    {
        string Name { get; }

        TimeSpan? PooledConnectionLifetime { get; }

        TimeSpan? PooledConnectionIdleTimeout { get; }

        int? MaxConnectionsPerServer { get; }
    }
}