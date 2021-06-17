#nullable enable

using System;
using System.Net.Http;

namespace GGroupp.Infra
{
    public interface ISocketsHttpHandlerProvider
    {
        SocketsHttpHandler GetOrCreate(string name, Action<SocketsHttpHandler>? configure = null);
    }
}