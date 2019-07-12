using System.Text.RegularExpressions;

namespace BingoX.Validation
{
    public class EmailValidation : IValidation<string>
    {
        readonly static Regex email = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        public bool Valid(string input)
        {
            return email.IsMatch(input);

        }
    }
}
