#nullable enable

using System;

namespace GGroupp.Infra
{
    public sealed record HttpClientConfiguration : IHttpClientConfiguration
    {
        internal static readonly TimeSpan DefaultTimeout;

        static HttpClientConfiguration() => DefaultTimeout = TimeSpan.FromSeconds(100);

        public string? BaseAddressUrl { get; set; }

        public TimeSpan? Timeout { get; set; }

        public bool IsLoggingDisabled { get; set; }

        Uri? IHttpClientConfiguration.BaseAddress => string.IsNullOrEmpty(BaseAddressUrl) ? null : new(BaseAddressUrl);

        TimeSpan IHttpClientConfiguration.Timeout => Timeout ?? DefaultTimeout;
    }
}