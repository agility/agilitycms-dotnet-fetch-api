namespace Agility.NET.FetchAPI.Helpers
{
	public static class UrlHelpers
	{
		public static string AppendParameter(string url, string parameter)
		{
			if (!url.Contains("?"))
			{
				//no query string yet
				return $"{url}?{parameter}";
			}

			//already has a query string
			return $"{url}&{parameter}";
		}
	}
}