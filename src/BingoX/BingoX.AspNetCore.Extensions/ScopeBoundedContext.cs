using BingoX.Generator;
using BingoX.Services;
using BingoX.AspNetCore;

namespace Microsoft.Extensions.DependencyInjection
{
    class ScopeBoundedContext : IBoundedContext
    {
        public string WebRootPath { get; internal set; }

        public string ContentRootPath { get; internal set; }

        public IGenerator<long> Generator { get; internal set; }

        public IDateTimeService DateTimeService { get; internal set; }

        public bool IsProduction { get; internal set; }
        public string AppVersion { get; internal set; }
        public string AppName { get; internal set; }
        public string OS { get; internal set; }
    }
}
