using System;
using System.Text.RegularExpressions;

namespace BingoX.VirtualEntity
{

    [Serializable]
    public class EmailException : Exception
    {
        public EmailException() { }
        public EmailException(string message) : base(message) { }
        public EmailException(string message, Exception inner) : base(message, inner) { }
        protected EmailException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    public class Email
    {
        private const string EMAIL_PATTERN = "^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$";
        private static readonly Regex PATTERN = new Regex(EMAIL_PATTERN);
        private readonly string value;
        public string Value { get { return value; } }

        public Email(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("message", nameof(value));
            }


            if (!PATTERN.IsMatch(value))
            {
                throw new EmailException("Email nao válido");
            }
            this.value = value;
        }

        public override bool Equals(object o)
        {
            if (this == null) return false;

            if (!typeof(Email).IsAssignableFrom(o.GetType()))
            {
                return false;
            }

            Email email = (Email)o;
            return string.Equals(value, email.value);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return value;
        }
        public static Email Of(string value)
        {

            return new Email(value);
        }

        public static bool operator ==(Email x, Email y)
        {
            return x.Equals(y);
        }
        public static bool operator !=(Email x, Email y)
        {
            return !x.Equals(y);
        }
    }
}
