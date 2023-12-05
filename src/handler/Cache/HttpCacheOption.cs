using System;
using System.Collections.ObjectModel;
using System.Net;

namespace GarageGroup.Infra;

public sealed record class HttpCacheOption
{
    public ReadOnlyDictionary<HttpStatusCode, TimeSpan>? ExpirationPerHttpResponseCode { get; init; }
}