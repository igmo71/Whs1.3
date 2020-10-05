using System;
using System.Numerics;

namespace Whs.Shared.Utils
{
    public static class GuidConvert
    {
        private static readonly string alphabet = "0123456789abcdef";
        public static string ToNumStr(string guidString)
        {
            string value = Guid.Parse(guidString).ToString("n");
            BigInteger bigInt = 0;

            for (int i = 0; i < value.Length; i++)
            {
                bigInt = bigInt * 16 + alphabet.IndexOf(value.Substring(i, 1));
            }

            string result = bigInt.ToString();
            result = result.Length % 2 != 0 ? $"0{result}" : result;
            return result;
        }
        public static string FromNumStr(string numStr)
        {
            _ = BigInteger.TryParse(numStr, out BigInteger bigInt);
            string result = "";

            while (bigInt > 0)
            {
                int remainder = (int)(bigInt % 16);
                bigInt = (bigInt - remainder) / 16;
                result = alphabet.Substring(remainder, 1) + result;
            }

            while (result.Length < 32)
                result = $"0{result}";

            _ = Guid.TryParse(result, out Guid guid);
            return guid.ToString();
        }
    }
}
