using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;

namespace GGroupp.Infra;

public sealed partial class DefaultSocketsHttpHandlerProvider : ISocketsHttpHandlerProvider, IDisposable
{
    private readonly ConcurrentDictionary<string, SocketsHttpHandler> namedHandlers = new();

    private bool disposed;

    public SocketsHttpHandler GetOrCreate([AllowNull] string name, Action<SocketsHttpHandler>? configure = null)
    {
        if (disposed)
        {
            throw new ObjectDisposedException($"{nameof(DefaultSocketsHttpHandlerProvider)} was disposed.");
        }
        return InternalGetOrCreate(name ?? string.Empty, configure);
    }

    private SocketsHttpHandler InternalGetOrCreate(string name, Action<SocketsHttpHandler>? configure)
        =>
        namedHandlers.GetOrAdd(
            name,
            _ =>
            {
                var handler = new SocketsHttpHandler();
                configure?.Invoke(handler);
                return handler;
            });


    public void Dispose()
    {
        if (disposed is false)
        {
            foreach (var handler in namedHandlers.Select(pair => pair.Value))
            {
                handler.Dispose();
            }
            disposed = true;
        }
    }
}
