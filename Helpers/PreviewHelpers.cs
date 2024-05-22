using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Agility.NET.FetchAPI.Util;
using Microsoft.AspNetCore.Http;

namespace Agility.NET.FetchAPI.Helpers
{
    public static class PreviewHelpers
    {

        public static string GenerateAgilityPreviewKey(string securityKey)
        {
            var data = Encoding.Unicode.GetBytes($"{-1}_{securityKey}_Preview");
            var shaM = SHA512.Create();
            var result = shaM.ComputeHash(data);
            return Convert.ToBase64String(result);
        }
    }
}