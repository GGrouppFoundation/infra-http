using System.Net.Http;
using Microsoft.Extensions.Caching.InMemory;
using Microsoft.Extensions.Logging;

namespace GarageGroup.Infra;

internal sealed partial class InMemoryCacheHttpHandler : DelegatingHandler
{
    private static readonly MemoryCache ResponseCache;

    private static readonly DefaultCacheKeysProvider CacheKeysProvider;

    static InMemoryCacheHttpHandler()
    {
        ResponseCache = new(new());
        CacheKeysProvider = new();
    }

    private readonly HttpCacheOption option;

    private readonly ILogger? logger;

    internal InMemoryCacheHttpHandler(HttpMessageHandler innerHandler, HttpCacheOption option, ILoggerFactory? loggerFactory) : base(innerHandler)
    {
        this.option = option;
        logger = loggerFactory?.CreateLogger<InMemoryCacheHttpHandler>();
    }
}