using System;

namespace GarageGroup.Infra;

partial class HttpConfigurationExtensions
{
    public static SocketsHttpHandlerConfiguration GetSocketsHttpHandlerConfigurationFromEnvironment(
        this IServiceProvider _, string sectionName)
        =>
        new()
        {
            Name = GetVariableFromSection(sectionName, "Name"),
            PooledConnectionLifetime = GetVariableFromSection(sectionName, "PooledConnectionLifetime").ParseTimeSpan(),
            PooledConnectionIdleTimeout = GetVariableFromSection(sectionName, "PooledConnectionIdleTimeout").ParseTimeSpan()
        };
}