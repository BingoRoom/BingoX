using System;

namespace BingoX.Generator
{
    public class InvalidSystemClock : Exception
    {
        public InvalidSystemClock(string message) : base(message) { }
    }
}
