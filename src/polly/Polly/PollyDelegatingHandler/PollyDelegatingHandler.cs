using System.Net.Http;
using Polly;

namespace GGroupp.Infra;

internal sealed partial class PollyDelegatingHandler : DelegatingHandler
{
    private readonly IAsyncPolicy<HttpResponseMessage> retryPolicy;

    internal PollyDelegatingHandler(HttpMessageHandler innerHandler, IAsyncPolicy<HttpResponseMessage> retryPolicy)
        : base(innerHandler)
        =>
        this.retryPolicy = retryPolicy;
}