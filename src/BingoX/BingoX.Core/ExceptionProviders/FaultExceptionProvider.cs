using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace BingoX.ExceptionProviders
{
    public abstract class FaultExceptionProvider
    {
        public abstract bool IsType(Exception exception);
        protected T GetException<T>(Exception exception) where T : Exception
        {
            if (exception is T) return (T)exception;
            Exception innerException = exception.InnerException;
            while (innerException != null)
            {
                if (innerException is T) return (T)innerException;
                innerException = innerException.InnerException;
            }

            return null;
        }
        protected bool IsType<T>(Exception exception) where T : Exception
        {
            if (exception is T) return true;
            Exception innerException = exception.InnerException;
            while (innerException != null)
            {
                if (innerException is T) return true;
                innerException = innerException.InnerException;
            }

            return false;
        }
        public abstract string GetMessage(Exception exception);
        readonly static IList<FaultExceptionProvider> providers = new List<FaultExceptionProvider>() {
            new GenericFaultExceptionProvider<NotImplementedException>("没实现此方法") ,
            new GenericFaultExceptionProvider<LogicException>() ,
            new GenericFaultExceptionProvider<WebException>() ,
        //    new GenericFaultExceptionProvider<System.Net.Sockets.SocketException>("连接服务器不成功") ,
            new GenericFaultExceptionProvider<IOException>("文件或读写功能不能處理") ,
        };
        public readonly static UnkownFaultExceptionProvider Unhandled = new UnkownFaultExceptionProvider();
        public static void Add(FaultExceptionProvider provider)
        {
            providers.Add(provider);
        }
        public static FaultExceptionProvider Get(Exception exception)
        {
            var provider = providers.FirstOrDefault(n => n.IsType(exception));
 
            return provider;
        }
    }
}
