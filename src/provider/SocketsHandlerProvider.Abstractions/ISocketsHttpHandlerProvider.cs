using System;
using System.Net.Http;

namespace GarageGroup.Infra;

public interface ISocketsHttpHandlerProvider
{
    SocketsHttpHandler GetOrCreate(string name, Action<SocketsHttpHandler>? configure = null);
}