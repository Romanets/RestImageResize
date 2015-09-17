using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RestImageResize.Security
{
    public class HashGenerator
    {
        public string ComputeHash(string privateKey, int width, int height, ImageTransform transform)
        {
            var values = new[] { privateKey, width.ToString(), height.ToString(), transform.ToString() };
            var bytes = Encoding.ASCII.GetBytes(string.Join(":", values));

            var sha1 = SHA1.Create();
            sha1.ComputeHash(bytes);

            return BitConverter.ToString(sha1.Hash).Replace("-", "").ToLower();
        }
    }
}