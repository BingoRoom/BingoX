using System;

namespace BingoX.ExceptionProviders
{
    public class UnkownFaultExceptionProvider : FaultExceptionProvider
    {
        public override string GetMessage(Exception exception)
        {
            return "操作失敗，請重試！";
        }

        public override bool IsType(Exception exception)
        {
            return true;
        }
    }
}
