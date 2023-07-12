namespace Agility.NET.FetchAPI.Helpers
{
    public static class UrlHelpers
    {
        public static bool DoesUrlEndWithCharacter(string url, string character)
        {
            return url.EndsWith(character);
        }

        public static string AppendParameter(string url, string parameter)
        {
            if (DoesUrlEndWithCharacter(url, "?"))
            {
                return url + parameter;
            }

            return url + $@"&{parameter}";
        }
    }
}