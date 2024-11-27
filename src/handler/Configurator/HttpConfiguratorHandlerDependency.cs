using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using PrimeFuncPack;

namespace GarageGroup.Infra;

public static class HttpConfiguratorHandlerDependency
{
    public static Dependency<HttpMessageHandler> ConfigureHttpHeader(
        this Dependency<HttpMessageHandler> dependency, [DisallowNull] string headerName, string configurationKey)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        ArgumentException.ThrowIfNullOrWhiteSpace(headerName);

        return dependency.Map<HttpMessageHandler>(CreateHandler);

        InternalHttpHeaderHandler CreateHandler(IServiceProvider serviceProvider, HttpMessageHandler innerHandler)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(innerHandler);

            return new InternalHttpHeaderHandler(
                innerHandler: innerHandler,
                headerName: headerName,
                headerValue: serviceProvider.GetServiceOrThrow<IConfiguration>()[configurationKey.OrEmpty()].OrEmpty());
        }
    }

    public static Dependency<HttpMessageHandler> ConfigureQueryParameter(
        this Dependency<HttpMessageHandler> dependency, [DisallowNull] string parameterName, string configurationKey)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        ArgumentException.ThrowIfNullOrWhiteSpace(parameterName);

        return dependency.Map<HttpMessageHandler>(CreateHandler);

        InternalQueryParameterHandler CreateHandler(IServiceProvider serviceProvider, HttpMessageHandler innerHandler)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(innerHandler);

            return new(
                innerHandler: innerHandler,
                parameterName: parameterName,
                parameterValue: serviceProvider.GetServiceOrThrow<IConfiguration>()[configurationKey.OrEmpty()].OrEmpty());
        }
    }

    private static string OrEmpty(this string? source)
        =>
        source ?? string.Empty;
}