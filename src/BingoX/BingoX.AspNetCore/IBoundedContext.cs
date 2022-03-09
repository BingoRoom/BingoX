using BingoX.Generator;
using BingoX.Services;
using System;

namespace BingoX.AspNetCore
{

    public interface IBoundedContext
    {
        string WebRootPath { get; }
        string ContentRootPath { get; }
        string AppVersion { get; }
        string AppName { get; }
        string OS { get; }

        bool IsProduction { get; }
        IGenerator<long> Generator { get; }
        IDateTimeService DateTimeService { get; }
    }

    /// <summary>
    /// 网站备案信息
    /// </summary>
    public interface IRecordFiling
    {
        string No { get; }
        string Company { get; }
        string CopyrightYear { get;  }
        string Url { get; }
    }
}
