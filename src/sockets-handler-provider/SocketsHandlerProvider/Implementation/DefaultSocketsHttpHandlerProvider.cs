using System;
using System.Collections.Concurrent;
using System.Net.Http;

namespace GGroupp.Infra;

internal sealed partial class DefaultSocketsHttpHandlerProvider : ISocketsHttpHandlerProvider, IDisposable
{
    private readonly ConcurrentDictionary<string, SocketsHttpHandler> namedHandlers = new();

    private bool disposed;
}
