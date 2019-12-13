using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCrateAPI.Helpers
{
    public static class Helper
    {
        public static string EncryptBase64(this string clearValue)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(clearValue);
            return Convert.ToBase64String(bytes);
        }
        public static string DecryptBase64(this string cipherText)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(cipherText);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
