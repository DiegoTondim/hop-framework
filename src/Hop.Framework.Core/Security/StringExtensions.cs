using System;
using System.Security.Cryptography;
using System.Text;

namespace Hop.Framework.Core.Security
{
    public static class StringExtensions
    {
        public static string Hash(this string input, bool upper = true)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            using (var algorithm = SHA384.Create())
            {
                var hashedBytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "");
                return upper ? hash.ToUpperInvariant() : hash.ToLowerInvariant();
            }
        }
    }
}
