using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra;

public sealed partial class LoggerDelegatingHandler : DelegatingHandler
{
    public static LoggerDelegatingHandler Create(HttpMessageHandler innerHandler, ILogger logger)
        =>
        new(
            innerHandler ?? throw new ArgumentNullException(nameof(innerHandler)),
            logger ?? throw new ArgumentNullException(nameof(logger)));

    private readonly ILogger logger;

    internal LoggerDelegatingHandler(HttpMessageHandler innerHandler, ILogger logger) : base(innerHandler)
        =>
        this.logger = logger;
}
