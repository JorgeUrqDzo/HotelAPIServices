namespace Hotel.WebApi.Formatters
{
    public class FullNameFormatter
    {
        public static string Format(string firstName, string lastName, string middleName = null, bool abbreviateMiddle = false)
        {
            if (string.IsNullOrWhiteSpace(middleName))
            {
                return $"{firstName} {lastName}";
            }
            else
            {
                return $"{firstName} {FormatMiddle(middleName, abbreviateMiddle)} {lastName}";
            }
        }

        public static string FormatSurnameFirst(string firstName, string lastName, string middleName = null, bool abbreviateMiddle = false)
        {
            if (string.IsNullOrWhiteSpace(middleName))
            {
                return $"{lastName}, {firstName}";
            }
            else
            {
                return $"{lastName}, {firstName} {FormatMiddle(middleName, abbreviateMiddle)}";
            }
        }

        static string FormatMiddle(string middleName, bool abbreviateMiddle)
        {
            if (string.IsNullOrWhiteSpace(middleName))
            {
                return string.Empty;
            }

            return abbreviateMiddle ? $"{middleName[0].ToString()}." : middleName;
        }
    }
}