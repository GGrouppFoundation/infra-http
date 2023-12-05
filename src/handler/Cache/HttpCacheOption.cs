using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace GarageGroup.Infra;

public sealed record class HttpCacheOption
{
    private static readonly ReadOnlyDictionary<HttpStatusCode, TimeSpan> DefaultExpirationPerHttpResponseCode;

    static HttpCacheOption()
        =>
        DefaultExpirationPerHttpResponseCode = new(
            new Dictionary<HttpStatusCode, TimeSpan>
            {
                [HttpStatusCode.OK] = TimeSpan.FromMinutes(60)
            });

    public HttpCacheOption([AllowNull] ReadOnlyDictionary<HttpStatusCode, TimeSpan> expirationPerHttpResponseCode)
        =>
        ExpirationPerHttpResponseCode = expirationPerHttpResponseCode?.Count > 0 ? expirationPerHttpResponseCode : DefaultExpirationPerHttpResponseCode;

    public ReadOnlyDictionary<HttpStatusCode, TimeSpan> ExpirationPerHttpResponseCode { get; }
}