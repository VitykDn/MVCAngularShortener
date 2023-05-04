using MVCAngularShortener.Infrastructure.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace MVCAngularShortener.Infrastructure.Services
{
    public class  UrlShortenerService : IShortener
    {

        private const string Alphabet = "23456789bcdfghjkmnpqrstvwxyzBCDFGHJKLMNPQRSTVWXYZ-_";
        private static readonly int Base = Alphabet.Length;

        public string Decode(string shortUrl)
        {
            throw new NotImplementedException();
        }

        public string Encode(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                string encodedString = Convert.ToBase64String(hashBytes)
                                                .Replace("/", "")
                                                .Replace("+", "")
                                                .Replace("=", "")
                                                .Substring(0, 6);
                return encodedString;
            }
        }

        //public string Decode(string input)
        //{
        //    var sb = new StringBuilder();
        //    var reversedInput = new string(input.Reverse().ToArray());

        //    var baseToPowers = new int[input.Length];
        //    baseToPowers[0] = 1;

        //    for (int i = 1; i < input.Length; i++)
        //    {
        //        baseToPowers[i] = baseToPowers[i - 1] * Base;
        //    }

        //    int num = 0;
        //    for (int i = 0; i < input.Length; i++)
        //    {
        //        int index = Alphabet.IndexOf(reversedInput[i]);
        //        if (index < 0)
        //        {
        //            throw new ArgumentException("Invalid input string");
        //        }

        //        num += index * baseToPowers[i];
        //    }

        //    var bytes = BitConverter.GetBytes(num);
        //    var decodedString = Encoding.UTF8.GetString(bytes);
        //    return decodedString;
        //}

    }
}
