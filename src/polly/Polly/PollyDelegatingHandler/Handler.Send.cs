using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class PollyDelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Debug.Assert(request is not null);
        return retryPolicy.ExecuteAsync(InvokeAsync);

        Task<HttpResponseMessage> InvokeAsync()
            =>
            cancellationToken.IsCancellationRequested switch
            {
                true => Task.FromCanceled<HttpResponseMessage>(cancellationToken),
                _ => base.SendAsync(request, cancellationToken)
            };
    }
}