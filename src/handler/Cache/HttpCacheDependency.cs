using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Caching.InMemory;
using Microsoft.Extensions.Configuration;
using PrimeFuncPack;

namespace GarageGroup.Infra;

public static class HttpCacheDependency
{
    private const string DefaultSectionName = "HttpCache";

    public static Dependency<HttpMessageHandler> UseInMemoryCache(
        this Dependency<HttpMessageHandler> dependency,
        Func<IServiceProvider, HttpCacheOption> optionResolver)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        ArgumentNullException.ThrowIfNull(optionResolver);

        return dependency.With(optionResolver).Fold<HttpMessageHandler>(CreateInMemoryCacheHandler);

        static InMemoryCacheHandler CreateInMemoryCacheHandler(HttpMessageHandler innerHandler, HttpCacheOption option)
        {
            ArgumentNullException.ThrowIfNull(innerHandler);
            ArgumentNullException.ThrowIfNull(option);

            return InnerCreateInMemoryCacheHandler(innerHandler, option);
        }
    }

    public static Dependency<HttpMessageHandler> UseInMemoryCache(
        this Dependency<HttpMessageHandler> dependency,
        string cacheSectionName = DefaultSectionName)
    {
        ArgumentNullException.ThrowIfNull(dependency);

        return dependency.Map<HttpMessageHandler>(ResolveHandler);

        InMemoryCacheHandler ResolveHandler(IServiceProvider serviceProvider, HttpMessageHandler innerHandler)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(innerHandler);

            var configuration = serviceProvider.GetServiceOrThrow<IConfiguration>().GetSection(cacheSectionName ?? string.Empty);
            return InnerCreateInMemoryCacheHandler(innerHandler, configuration.GetHttpCacheOption());
        }
    }

    private static HttpCacheOption GetHttpCacheOption(this IConfiguration configuration)
    {
        var section = configuration.GetSection(nameof(HttpCacheOption.ExpirationPerHttpResponseCode));
        var expirationPerHttpResponseCode = new Dictionary<HttpStatusCode, TimeSpan>();

        foreach (var codeSection in section.GetChildren())
        {
            var value = codeSection.Value;
            if (string.IsNullOrEmpty(value))
            {
                continue;
            }

            var statusCode = Enum.Parse<HttpStatusCode>(codeSection.Key, ignoreCase: true);
            expirationPerHttpResponseCode[statusCode] = TimeSpan.Parse(value);
        }

        return new(
            expirationPerHttpResponseCode: new(expirationPerHttpResponseCode));
    }

    private static InMemoryCacheHandler InnerCreateInMemoryCacheHandler(HttpMessageHandler innerHandler, HttpCacheOption option)
        =>
        new(innerHandler, option.ExpirationPerHttpResponseCode);
}