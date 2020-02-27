using System;
using System.Linq;

namespace BingoX.Validation
{
    public class CreditCardValidation : IValidation<string>
    {
        public bool Valid(string value)
        {
            var digits = GetDigitsArrayFromCardNumber(value);
            return IsLuhnValid(digits);
        }
        bool IsLuhnValid(int[] digits)
        {
            var sum = 0;
            var alt = false;
            for (var i = digits.Length - 1; i >= 0; i--)
            {
                if (alt)
                {
                    digits[i] *= 2;
                    if (digits[i] > 9)
                    {
                        digits[i] -= 9;
                    }
                }
                sum += digits[i];
                alt = !alt;
            }

            return sum % 10 == 0;
        }
        int[] GetDigitsArrayFromCardNumber(string cardNumber)
        {
            var digits = cardNumber.Select(p => int.Parse(p.ToString())).ToArray();
            return digits;
        }
    }
}
