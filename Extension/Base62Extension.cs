using System.Text;

namespace UrlShortener.Base64Extension
{
    public static class Base62Extension
    {
        private const string Alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static string ToBase62(this long number)
        {
            if (number == 0) return "0";

            var sb = new StringBuilder();
            while (number > 0)
            {
                int remainder = (int)(number % 62);
                sb.Insert(0, Alphabet[remainder]);
                number /= 62;
            }

            return sb.ToString();
        }

        public static long FromBase62(string base62)
        {
            long result = 0;
            foreach (char c in base62)
            {
                int value = Alphabet.IndexOf(c);
                result = result * 62 + (long)value;
            }
            return result;
        }
    }
}
