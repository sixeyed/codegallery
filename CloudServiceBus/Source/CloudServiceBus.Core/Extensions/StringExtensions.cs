using System;
using System.Security.Cryptography;
using System.Text;

namespace CloudServiceBus.Core.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns a deterministic GUID from the given string
        /// </summary>
        /// <remarks>
        /// Seems to be required in every solution, no matter how small
        /// http://geekswithblogs.net/EltonStoneman/archive/2008/06/26/generating-deterministic-guids.aspx
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDeterministicGuid(this string value)
        {
            //use MD5 hash to get a 16-byte hash of the string: 
            var provider = new MD5CryptoServiceProvider();
            byte[] inputBytes = Encoding.Default.GetBytes(value);
            byte[] hashBytes = provider.ComputeHash(inputBytes);
            //generate a guid from the hash: 
            Guid hashGuid = new Guid(hashBytes);
            return hashGuid.ToString();
        }
    }
}
