#nullable enable

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra
{
    partial class LoggerDelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            logger.LogInformation($"Sending request {request.Method.Method} {request.RequestUri}");
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            logger.LogInformation($"Received response {response.StatusCode} {response.Content?.Headers?.ContentLength} bytes.");

            return response;
        }
    }
}