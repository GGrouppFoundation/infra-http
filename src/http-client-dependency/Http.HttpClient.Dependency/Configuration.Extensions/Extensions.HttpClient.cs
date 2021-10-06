using System;

namespace GGroupp.Infra;

partial class HttpConfigurationExtensions
{
    public static HttpClientConfiguration GetHttpClientConfigurationFromEnvironment(
        this IServiceProvider _, string sectionName)
        =>
        new()
        {
            BaseAddressUrl = GetVariableFromSection(sectionName, "BaseAddressUrl"),
            Timeout = GetVariableFromSection(sectionName, "Timeout").Pipe(ParseTimeSpan),
            IsLoggingDisabled = GetVariableFromSection(sectionName, "IsLoggingDisabled").Pipe(ParseBool) ?? false
        };
}
