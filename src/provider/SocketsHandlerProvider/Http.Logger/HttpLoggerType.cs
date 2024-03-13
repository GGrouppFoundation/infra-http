using System;

namespace GarageGroup.Infra;

[Flags]
public enum HttpLoggerType
{
    Default = 0,

    RequestHeaders = 1,

    RequestBody = 2,

    ResponseHeaders = 4,

    ResponseBody = 8
}