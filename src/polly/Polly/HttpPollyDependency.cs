using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using Polly;
using Polly.Extensions.Http;
using PrimeFuncPack;

namespace GGroupp.Infra;

public static class HttpPollyDependency
{
    private const int StandardRetryCount = 5;

    public static Dependency<HttpMessageHandler> UsePollyStandard(
        this Dependency<HttpMessageHandler> dependency, params HttpStatusCode[] statusCodes)
    {
        ArgumentNullException.ThrowIfNull(dependency);

        var retryPolicy = GetStandardRetryPolicy(StandardRetryCount, statusCodes);
        return dependency.Map<HttpMessageHandler>(CreateHandler);

        PollyDelegatingHandler CreateHandler(HttpMessageHandler innerHandler)
        {
            ArgumentNullException.ThrowIfNull(innerHandler);
            return new(innerHandler, retryPolicy);
        }
    }

    public static Dependency<HttpMessageHandler> UsePolly(
        this Dependency<HttpMessageHandler> dependency,
        Func<IServiceProvider, IAsyncPolicy<HttpResponseMessage>> retryPollyResolver)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        ArgumentNullException.ThrowIfNull(retryPollyResolver);

        return dependency.Map<HttpMessageHandler>(CreateHandler);

        PollyDelegatingHandler CreateHandler(IServiceProvider serviceProvider, HttpMessageHandler innerHandler)
        {
            Debug.Assert(serviceProvider is not null);
            return CreatePollyDelegatingHandler(innerHandler, retryPollyResolver.Invoke(serviceProvider));
        }
    }

    public static Dependency<HttpMessageHandler> UsePolly(
        this Dependency<HttpMessageHandler> dependency,
        Func<IAsyncPolicy<HttpResponseMessage>> retryPollyFactory)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        ArgumentNullException.ThrowIfNull(retryPollyFactory);

        return dependency.Map<HttpMessageHandler>(CreateHandler);

        PollyDelegatingHandler CreateHandler(HttpMessageHandler innerHandler)
            =>
            CreatePollyDelegatingHandler(innerHandler, retryPollyFactory.Invoke());
    }

    public static Dependency<HttpMessageHandler> UsePolly(
        this Dependency<HttpMessageHandler> dependency, IAsyncPolicy<HttpResponseMessage> retryPolicy)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        ArgumentNullException.ThrowIfNull(retryPolicy);

        return dependency.Map<HttpMessageHandler>(CreateHandler);

        PollyDelegatingHandler CreateHandler(HttpMessageHandler innerHandler)
            =>
            CreatePollyDelegatingHandler(innerHandler, retryPolicy);
    }

    public static Dependency<HttpMessageHandler> UsePolly(
        this Dependency<HttpMessageHandler, IAsyncPolicy<HttpResponseMessage>> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold<HttpMessageHandler>(CreatePollyDelegatingHandler);
    }

    private static PollyDelegatingHandler CreatePollyDelegatingHandler(
        HttpMessageHandler innerHandler, IAsyncPolicy<HttpResponseMessage> retryPolicy)
    {
        ArgumentNullException.ThrowIfNull(innerHandler);
        ArgumentNullException.ThrowIfNull(retryPolicy);

        return new(innerHandler, retryPolicy);
    }

    private static IAsyncPolicy<HttpResponseMessage> GetStandardRetryPolicy(int retryCount, [AllowNull] HttpStatusCode[] statusCodes)
    {
        var builder = HttpPolicyExtensions.HandleTransientHttpError();

        if (statusCodes?.Length > 0)
        {
            builder = builder.OrResult(IsStatusCodeRetried);
        }

        return builder.WaitAndRetryAsync(retryCount, GetSleepDuration);

        bool IsStatusCodeRetried(HttpResponseMessage response)
            =>
            statusCodes.Contains(response.StatusCode);

        static TimeSpan GetSleepDuration(int retryAttempt)
            =>
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
    }
}