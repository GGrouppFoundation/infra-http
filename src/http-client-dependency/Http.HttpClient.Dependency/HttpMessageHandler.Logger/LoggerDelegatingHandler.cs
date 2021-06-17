#nullable enable

using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra
{
    public sealed partial class LoggerDelegatingHandler : DelegatingHandler
    {
        public static LoggerDelegatingHandler Create(ILogger logger, HttpMessageHandler innerHandler)
            =>
            new(
                logger ?? throw new ArgumentNullException(nameof(logger)),
                innerHandler ?? throw new ArgumentNullException(nameof(innerHandler)));

        private readonly ILogger logger;

        internal LoggerDelegatingHandler(ILogger logger, HttpMessageHandler innerHandler) : base(innerHandler)
            =>
            this.logger = logger;
    }
}