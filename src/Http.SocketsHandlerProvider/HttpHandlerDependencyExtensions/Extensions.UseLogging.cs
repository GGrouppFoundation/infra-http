using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using PrimeFuncPack;

namespace GGroupp.Infra;

partial class HttpHandlerDependencyExtensions
{
    public static Dependency<HttpMessageHandler> UseLogging<THandler>(
        this Dependency<THandler> sourceDependency, Func<IServiceProvider, ILogger> loggerResolver)
        where THandler : HttpMessageHandler
    {
        _ = sourceDependency ?? throw new ArgumentNullException(nameof(sourceDependency));
        _ = loggerResolver ?? throw new ArgumentNullException(nameof(loggerResolver));

        return sourceDependency.With(loggerResolver).Fold<HttpMessageHandler>(LoggerDelegatingHandler.Create);
    }

    public static Dependency<HttpMessageHandler> UseLogging<THandler>(
        this Dependency<THandler, ILogger> sourceDependency)
        where THandler : HttpMessageHandler
    {
        _ = sourceDependency ?? throw new ArgumentNullException(nameof(sourceDependency));

        return sourceDependency.Fold<HttpMessageHandler>(LoggerDelegatingHandler.Create);
    }
}
