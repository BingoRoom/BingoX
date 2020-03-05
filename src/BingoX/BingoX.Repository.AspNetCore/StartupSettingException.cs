using System;

namespace BingoX.Repository.AspNetCore
{
    [Serializable]
    public class StartupSettingException : LogicException
    {
        public StartupSettingException() { }
        public StartupSettingException(string message) : base(message) { }
        public StartupSettingException(string message, Exception inner) : base(message, inner) { }
        protected StartupSettingException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
