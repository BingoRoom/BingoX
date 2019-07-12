using BingoX.Services;
using System;

namespace BingoX.Core.Test
{
    class SubDateTimeService : DateTimeService
    {
        DateTime currnetNow;

        public SubDateTimeService(DateTime now)
        {
            SetNow(now);
        }
        public void SetNow(DateTime now)
        {
            currnetNow = now;
        }
        public override DateTime GetNow()
        {
            return currnetNow;
        }


    }
}
