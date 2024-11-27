using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace GarageGroup.Infra;

partial class DefaultSocketsHttpHandlerProvider
{
    public SocketsHttpHandler GetOrCreate([AllowNull] string name, Action<SocketsHttpHandler>? configure = null)
    {
        return disposed
            ? throw new ObjectDisposedException($"{nameof(DefaultSocketsHttpHandlerProvider)} was disposed.")
            : namedHandlers.GetOrAdd(name ?? string.Empty,CreateHandler);

        SocketsHttpHandler CreateHandler(string _)
        {
            var handler = new SocketsHttpHandler();
            configure?.Invoke(handler);

            return handler;
        }
    }
}