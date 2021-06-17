#nullable enable

using System;

namespace GGroupp.Infra
{
    public interface IHttpClientConfiguration
    {
        Uri? BaseAddress { get; }

        TimeSpan Timeout { get; }

        bool IsLoggingDisabled { get; }
    }
}