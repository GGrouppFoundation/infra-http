using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

internal sealed class InternalHttpHeaderHandler : DelegatingHandler
{
    private readonly string headerName;

    private readonly string headerValue;

    internal InternalHttpHeaderHandler(HttpMessageHandler innerHandler, string headerName, string headerValue) : base(innerHandler)
    {
        this.headerName = headerName;
        this.headerValue = headerValue;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Debug.Assert(request is not null);

        if (request.Headers.Contains(headerName))
        {
            request.Headers.Remove(headerName);
        }

        request.Headers.Add(headerName, headerValue);
        return base.SendAsync(request, cancellationToken);
    }
}