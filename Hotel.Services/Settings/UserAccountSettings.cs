namespace Hotel.Services.Settings
{
    public class UserAccountSettings
    {
        public int MinimumLength { get; set; }
        public int NumberOfCapitalLetters { get; set; }
        public int NumberOfDigits { get; set; }
        public int NumberOfSpecialChars { get; set; }
        public int PasswordExpirationDuration { get; set; }
        public int PasswordNumPrevAllowed { get; set; }
        public string PasswordSpecialCharacters { get; set; }
        public int NumFailedLoginAttemptsLockout { get; set; }
    }
}