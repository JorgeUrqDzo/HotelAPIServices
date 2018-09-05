using System.Linq;
using System.Text.RegularExpressions;
using Hotel.Services.Settings;

namespace Hotel.Services.Helpers
{
    internal class PasswordComplexityValidator
    {
        UserAccountSettings Settings { get; set; }
        string Password { get; set; }

        public PasswordComplexityValidator(UserAccountSettings userAccountSettings, string password)
        {
            Settings = userAccountSettings;
            Password = password;
        }

        public bool IsValid()
        {
            return CheckMinimumLength()
                   && CheckNumberOfCapitalLetters()
                   && CheckNumberOfDigits()
                   && CheckSpecialChars();
        }

        private bool CheckNumberOfCapitalLetters()
        {
            return Settings.NumberOfCapitalLetters == 0
                   || Password.Count(char.IsUpper) >= Settings.NumberOfCapitalLetters;
        }

        private bool CheckSpecialChars()
        {
            if (Settings.NumberOfSpecialChars == 0)
            {
                return true;
            }

            string pattern = @"[ !""#$%&'() * +,\-.\/\\:;<=>?@[\]^_`{|}~]";

            var regex = new Regex(pattern);
            var matches = regex.Matches(Password);

            return matches.Count >= Settings.NumberOfSpecialChars;
        }

        private bool CheckMinimumLength()
        {
            return string.IsNullOrEmpty(Password) == false
                   && Password.Length >= Settings.MinimumLength;
        }

        private bool CheckNumberOfDigits()
        {
            return Settings.NumberOfDigits == 0
                   || Password.Count(char.IsDigit) >= Settings.NumberOfDigits;
        }
    }
}