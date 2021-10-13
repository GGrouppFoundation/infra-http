using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace GGroupp.Infra;

partial class DefaultSocketsHttpHandlerProvider
{
    public SocketsHttpHandler GetOrCreate([AllowNull] string name, Action<SocketsHttpHandler>? configure = null)
    {
        if (disposed)
        {
            throw new ObjectDisposedException($"{nameof(DefaultSocketsHttpHandlerProvider)} was disposed.");
        }
        return InnerGetOrCreate(name ?? string.Empty, configure);
    }

    private SocketsHttpHandler InnerGetOrCreate(string name, Action<SocketsHttpHandler>? configure)
        =>
        namedHandlers.GetOrAdd(
            name,
            _ =>
            {
                var handler = new SocketsHttpHandler();
                configure?.Invoke(handler);
                return handler;
            });
}
