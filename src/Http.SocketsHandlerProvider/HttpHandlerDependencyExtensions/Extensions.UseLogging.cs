using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using PrimeFuncPack;

namespace GGroupp.Infra;

partial class HttpHandlerDependencyExtensions
{
    public static Dependency<LoggerDelegatingHandler> UseLogging<THandler>(
        this Dependency<THandler> dependency, Func<IServiceProvider, ILogger> loggerResolver)
        where THandler : HttpMessageHandler
        =>
        InnerUseLogging(
            dependency ?? throw new ArgumentNullException(nameof(dependency)),
            loggerResolver ?? throw new ArgumentNullException(nameof(loggerResolver)));

    private static Dependency<LoggerDelegatingHandler> InnerUseLogging<THandler>(
        Dependency<THandler> dependency, Func<IServiceProvider, ILogger> loggerResolver)
        where THandler : HttpMessageHandler
        =>
        dependency.With(loggerResolver).Fold(LoggerDelegatingHandler.Create);
}
