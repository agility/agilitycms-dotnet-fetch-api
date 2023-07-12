using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Agility.NET.FetchAPI.Util;
using Microsoft.AspNetCore.Http;

namespace Agility.NET.FetchAPI.Helpers
{
    public static class HttpContextHelpers
    {
        public static string IsPreview(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor.HttpContext == null) return null;

            var isPreview = httpContextAccessor.HttpContext.Request.Cookies[Constants.IsPreviewCookieName]?.ToLower();

            if (!string.IsNullOrEmpty(isPreview)) return isPreview;

            httpContextAccessor.HttpContext.Response.Headers.TryGetValue("Set-Cookie", out var responseHeader);
            var setCookie = responseHeader.FirstOrDefault();

            if (setCookie == null) return isPreview;

            if (setCookie.Contains($@"{Constants.IsPreviewCookieName}=true"))
            {
                isPreview = "true";
            }

            return isPreview;
        }

        public static void SetPreviewCookie(IHttpContextAccessor httpContextAccessor)
        {
            httpContextAccessor.HttpContext?.Response.Cookies.Delete("isPreview");
            httpContextAccessor.HttpContext?.Response.Cookies.Append(
                Constants.IsPreviewCookieName,
                "true",
                new CookieOptions()
                {
                    Path = "/"
                }
            );
        }

        public static string GenerateAgilityPreviewKey(string securityKey)
        {
            var data = Encoding.Unicode.GetBytes($"{-1}_{securityKey}_Preview");
            var shaM = new SHA512Managed();
            var result = shaM.ComputeHash(data);
            return Convert.ToBase64String(result);
        }
    }
}