using System.Security.Cryptography;
using System.Text;

namespace API.Extentions
{
    public static class VoucherExtentions
    {
        // calculate value
        public static decimal CalculateVoucherValue(int points)
        {
            const decimal exchangeRate = 0.005M; // 1 đô la cho mỗi 200 điểm

            decimal voucherValue = points * exchangeRate;

            return voucherValue;
        }

        // auto gen Code
        public static string GenerateVoucherCode(int length)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            byte[] randomBytes = new byte[length];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            StringBuilder voucherCodeBuilder = new StringBuilder(length);
            foreach (byte b in randomBytes)
            {
                voucherCodeBuilder.Append(validChars[b % (validChars.Length)]);
            }

            return voucherCodeBuilder.ToString();
        }
    }
}
