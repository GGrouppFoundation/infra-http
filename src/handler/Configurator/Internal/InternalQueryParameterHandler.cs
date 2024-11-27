using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace GarageGroup.Infra;

internal sealed class InternalQueryParameterHandler : DelegatingHandler
{
    private readonly string parameterName;

    private readonly string parameterValue;

    internal InternalQueryParameterHandler(HttpMessageHandler innerHandler, string parameterName, string parameterValue) : base(innerHandler)
    {
        this.parameterName = parameterName;
        this.parameterValue = parameterValue;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Debug.Assert(request is not null);

        if (request.RequestUri is not null)
        {
            request.RequestUri = AddOrUpdateQueryParameter(request.RequestUri);
        }

        return base.SendAsync(request, cancellationToken);
    }

    private Uri AddOrUpdateQueryParameter(Uri uri)
    {
        var uriBuilder = new UriBuilder(uri);

        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query[parameterName] = parameterValue;

        uriBuilder.Query = query.ToString();
        return uriBuilder.Uri;
    }
}