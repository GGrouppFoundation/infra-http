using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PrimeFuncPack;

namespace GarageGroup.Infra;

partial class HttpHandlerDependencyExtensions
{
    public static Dependency<HttpMessageHandler> UseLogging<THandler>(
        this Dependency<THandler> sourceDependency, string logCategoryName)
        where THandler : HttpMessageHandler
    {
        ArgumentNullException.ThrowIfNull(sourceDependency);

        return sourceDependency.With(ResolveLogger).Fold<HttpMessageHandler>(LoggerDelegatingHandler.Create);

        ILogger ResolveLogger(IServiceProvider serviceProvider)
            =>
            serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(logCategoryName);
    }

    public static Dependency<HttpMessageHandler> UseLogging<THandler>(
        this Dependency<THandler> sourceDependency, Func<IServiceProvider, ILogger> loggerResolver)
        where THandler : HttpMessageHandler
    {
        ArgumentNullException.ThrowIfNull(sourceDependency);
        ArgumentNullException.ThrowIfNull(loggerResolver);

        return sourceDependency.With(loggerResolver).Fold<HttpMessageHandler>(LoggerDelegatingHandler.Create);
    }

    public static Dependency<HttpMessageHandler> UseLogging<THandler>(
        this Dependency<THandler, ILogger> sourceDependency)
        where THandler : HttpMessageHandler
    {
        ArgumentNullException.ThrowIfNull(sourceDependency);

        return sourceDependency.Fold<HttpMessageHandler>(LoggerDelegatingHandler.Create);
    }
}
