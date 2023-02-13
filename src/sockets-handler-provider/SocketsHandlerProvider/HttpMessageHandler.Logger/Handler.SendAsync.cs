using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra;

partial class LoggerDelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Debug.Assert(request is not null);

        var requestMethod = request.Method.Method;
        var requestUri = request.RequestUri;

        logger.LogInformation("Sending request {requestMethod} {requestUri}", requestMethod, requestUri);

        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        var responseStatusCode = response.StatusCode;
        var responseContentLength = response.Content?.Headers?.ContentLength;

        if (responseContentLength is not null)
        {
            logger.LogInformation("Received response {responseStatusCode} {responseContentLength} bytes", responseStatusCode, responseContentLength);
        }
        else
        {
            logger.LogInformation("Received response {responseStatusCode}", responseStatusCode);
        }

        return response;
    }
}
