using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GarageGroup.Infra;

public sealed class LoggerDelegatingHandler : DelegatingHandler
{
    private readonly ILogger logger;

    private readonly HttpLoggerType loggerType;

    internal LoggerDelegatingHandler(HttpMessageHandler innerHandler, ILogger logger, HttpLoggerType loggerType) : base(innerHandler)
    {
        this.logger = logger;
        this.loggerType = loggerType;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Debug.Assert(request is not null);

        var requestMethod = request.Method.Method;
        var requestUri = request.RequestUri;

        logger.LogInformation("Sending request {requestMethod} {requestUri}", requestMethod, requestUri);

        if (loggerType.HasFlag(HttpLoggerType.RequestHeaders))
        {
            foreach (var header in request.Headers)
            {
                logger.LogInformation("Request header '{headerName}: {headerValue}'", header.Key, string.Join(',', header.Value));
            }
        }

        if (request.Content is not null && loggerType.HasFlag(HttpLoggerType.RequestBody))
        {
            var requestBody = await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("Request body '{requestBody}'", requestBody);
        }

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

        if (loggerType.HasFlag(HttpLoggerType.ResponseHeaders))
        {
            foreach (var header in response.Headers)
            {
                logger.LogInformation("Response header '{headerName}: {headerValue}'", header.Key, string.Join(',', header.Value));
            }
        }

        if (response.Content is not null && loggerType.HasFlag(HttpLoggerType.ResponseBody))
        {
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("Response body '{responseBody}'", responseBody);
        }

        return response;
    }
}