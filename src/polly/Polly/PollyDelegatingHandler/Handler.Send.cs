using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class PollyDelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Debug.Assert(request is not null);
        return retryPolicy.ExecuteAsync(InvokeAsync, cancellationToken);

        Task<HttpResponseMessage> InvokeAsync(CancellationToken cancellationToken)
            =>
            cancellationToken.IsCancellationRequested switch
            {
                true => Task.FromCanceled<HttpResponseMessage>(cancellationToken),
                _ => base.SendAsync(request, cancellationToken)
            };
    }
}