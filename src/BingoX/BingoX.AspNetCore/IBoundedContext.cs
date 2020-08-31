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
}
