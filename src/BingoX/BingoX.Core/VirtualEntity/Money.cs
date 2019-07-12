using System;

namespace BingoX.VirtualEntity
{
    public struct Money : IComparable<Money>
    {
        private readonly decimal value;
        private readonly string currency;
        public decimal Value { get { return value; } }
        public string Currency { get { return currency; } }
        public Money(decimal value) : this("CNY", value)
        {

        }
        public Money(string currency, decimal value)
        {
            if (string.IsNullOrEmpty(currency))
            {
                throw new ArgumentException("message", nameof(currency));
            }

            this.currency = currency;
            this.value = value;
        }
        public bool IsMatch(Money money,int digits)
        {
            if (!string.Equals(this.Currency, money.Currency)) throw new MoneyException("货币种类不一致");
            return Math.Round(value, digits) == Math.Round(money.value, digits);
         //   return false;
        }
        public override bool Equals(object o)
        {
            if (o == null) return false;

            if (!typeof(Money).IsAssignableFrom(o.GetType()))
            {
                return false;
            }

            Money money = (Money)o;
            if (!string.Equals(currency, money.Currency)) return false;
            return value == money.value;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", currency, value);
        }
        public static implicit operator Money(decimal value)
        {
            return new Money(value);
        }
        public static implicit operator Money(double value)
        {
            return new Money((decimal)value);
        }

        public static Money operator +(Money x, Money y)
        {
            if (!string.Equals(x.Currency, y.Currency)) throw new MoneyException("货币种类不一致");
            return new Money(x.Currency, x.value + y.value);
        }
        public static Money operator -(Money x, Money y)
        {
            if (!string.Equals(x.Currency, y.Currency)) throw new MoneyException("货币种类不一致");
            return new Money(x.Currency, x.value - y.value);
        }
        public static bool operator ==(Money x, Money y)
        {
            return x.Equals(y);
        }
        public static bool operator !=(Money x, Money y)
        {
            return !x.Equals(y);
        }
        public int CompareTo(Money o)
        {
            if (o == null) return -1;
            if (!string.Equals(currency, o.Currency)) return -1;
            return value.CompareTo(o.value);
        }

        public static bool operator <(Money d1, Money d2)
        {
            return d1.CompareTo(d2) < 0;
        }

        public static bool operator >(Money d1, Money d2)
        {
            return d1.CompareTo(d2) > 0;
        }

        public static bool operator <=(Money d1, Money d2)
        {
            return d1.CompareTo(d2) <= 0;
        }

        public static bool operator >=(Money d1, Money d2)
        {
            return d1.CompareTo(d2) >= 0;
        }
    }

    [Serializable]
    public class MoneyException : Exception
    {
        public MoneyException() { }
        public MoneyException(string message) : base(message) { }
        public MoneyException(string message, Exception inner) : base(message, inner) { }
        protected MoneyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
