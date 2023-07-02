using System;

namespace GarageGroup.Infra;

public interface ISocketsHttpHandlerConfiguration
{
    string Name { get; }

    TimeSpan? PooledConnectionLifetime { get; }

    TimeSpan? PooledConnectionIdleTimeout { get; }

    int? MaxConnectionsPerServer { get; }
}
