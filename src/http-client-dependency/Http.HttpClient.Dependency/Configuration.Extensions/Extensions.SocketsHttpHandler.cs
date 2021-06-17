#nullable enable

using System;

namespace GGroupp.Infra
{
    partial class HttpConfigurationExtensions
    {
        public static SocketsHttpHandlerConfiguration GetSocketsHttpHandlerConfigurationFromEnvironment(
            this IServiceProvider serviceProvider, string sectionName)
            =>
            new()
            {
                Name = GetVariableFromSection(sectionName, "Name"),
                PooledConnectionLifetime = GetVariableFromSection(sectionName, "PooledConnectionLifetime").Pipe(ParseTimeSpan),
                PooledConnectionIdleTimeout = GetVariableFromSection(sectionName, "PooledConnectionIdleTimeout").Pipe(ParseTimeSpan)
            };
    }
}