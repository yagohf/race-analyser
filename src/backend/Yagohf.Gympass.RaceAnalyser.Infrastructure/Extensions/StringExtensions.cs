using System.Security.Cryptography;
using System.Text;

namespace Yagohf.Gympass.RaceAnalyser.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string ToCipherText(this string text)
        {
            //Utilizado MD5 apenas para propósito de demonstraçao.
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }
}
