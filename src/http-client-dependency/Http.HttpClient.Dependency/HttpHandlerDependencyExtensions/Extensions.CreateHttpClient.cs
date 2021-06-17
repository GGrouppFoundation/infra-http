#nullable enable

using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using PrimeFuncPack;

namespace GGroupp.Infra
{
    partial class HttpHandlerDependencyExtensions
    {
        public static Dependency<HttpClient> CreateHttpClient<THandler>(
            this Dependency<THandler, ILoggerFactory> dependency, Func<IServiceProvider, IHttpClientConfiguration> configurationResolver)
            where THandler : HttpMessageHandler
            =>
            InternalCreateHttpClient(
                dependency ?? throw new ArgumentNullException(nameof(dependency)),
                configurationResolver ?? throw new ArgumentNullException(nameof(configurationResolver)));

        public static Dependency<HttpClient> CreateHttpClient<THandler>(
            this Dependency<THandler> handlerDependency, Func<IServiceProvider, IHttpClientConfiguration> configurationResolver)
            where THandler : HttpMessageHandler
            =>
            InternalCreateHttpClient(
                handlerDependency ?? throw new ArgumentNullException(nameof(handlerDependency)),
                configurationResolver ?? throw new ArgumentNullException(nameof(configurationResolver)));

        private static Dependency<HttpClient> InternalCreateHttpClient<THandler>(
            Dependency<THandler, ILoggerFactory> dependency, Func<IServiceProvider, IHttpClientConfiguration> configurationResolver)
            where THandler : HttpMessageHandler
            =>
            dependency.With(configurationResolver).Fold(CreateHttpClient);

        private static Dependency<HttpClient> InternalCreateHttpClient<THandler>(
            Dependency<THandler> handlerDependency, Func<IServiceProvider, IHttpClientConfiguration> configurationResolver)
            where THandler : HttpMessageHandler
            =>
            handlerDependency.With(GetLoggerFactoryOrNull).With(configurationResolver).Fold(CreateHttpClient);

        private static ILoggerFactory? GetLoggerFactoryOrNull(IServiceProvider serviceProvider)
            =>
            serviceProvider.GetServiceOrAbsent<ILoggerFactory>().OrDefault();

        private static HttpClient CreateHttpClient(
            HttpMessageHandler httpMessageHandler, ILoggerFactory? loggerFactory, IHttpClientConfiguration? configuration)
            =>
            Pipeline.Pipe(
                loggerFactory is null || configuration?.IsLoggingDisabled is true
                    ? httpMessageHandler
                    : new LoggerDelegatingHandler(loggerFactory.CreateLogger(nameof(HttpClient)), httpMessageHandler))
            .Pipe(
                handler => new HttpClient(handler: handler, disposeHandler: false)
                {
                    BaseAddress = configuration?.BaseAddress,
                    Timeout = configuration?.Timeout ?? HttpClientConfiguration.DefaultTimeout
                });
    }
}