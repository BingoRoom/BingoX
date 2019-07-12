using System;

namespace BingoX.ExceptionProviders
{
    public class GenericFaultExceptionProvider<TException> : FaultExceptionProvider where TException : Exception
    {
        private readonly string message;
        public GenericFaultExceptionProvider()
        {

        }
        public GenericFaultExceptionProvider(string message)
        {
            this.message = message;
        }
        public override string GetMessage(Exception exception)
        {
            if (!string.IsNullOrEmpty(message)) return message;
            var ex = GetException<TException>(exception);
            return ex.Message;
        }

        public override bool IsType(Exception exception)
        {
            return this.IsType<TException>(exception);
        }
    }
}
