﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Utils
{
    public class Helper
    {
        public readonly static string AppKey = "1234567890abcdefgh";
        public readonly static string Issuer = "mysite.com";

        public static string GenHash(string input)
        {
            return string.Join("", new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input))
                .Select(x => x.ToString("X2")).ToArray());
        }
    }
}
