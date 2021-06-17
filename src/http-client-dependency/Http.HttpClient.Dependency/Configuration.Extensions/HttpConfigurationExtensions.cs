#nullable enable

using System;

namespace GGroupp.Infra
{
    public static partial class HttpConfigurationExtensions
    {
        private static string? GetVariableFromSection(string? sectionName, string variableName)
            =>
            string.IsNullOrEmpty(sectionName)
                ? Environment.GetEnvironmentVariable(variableName)
                : Environment.GetEnvironmentVariable(sectionName + ":" + variableName);

        private static TimeSpan? ParseTimeSpan(string? value)
            =>
            string.IsNullOrEmpty(value) ? null : TimeSpan.Parse(value);

        private static int? ParseInt(string? value)
            =>
            string.IsNullOrEmpty(value) ? null : int.Parse(value);

        private static bool? ParseBool(string? value)
            =>
            string.IsNullOrEmpty(value) ? null : bool.Parse(value);
    }
}