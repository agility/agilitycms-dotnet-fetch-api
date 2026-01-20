using System;

namespace Agility.NET.FetchAPI.Exceptions
{

	public class AgilityResponseException : Exception
	{
		public AgilityResponseException()
		{
		}

		public AgilityResponseException(string message) : base(message)
		{
		}

		public AgilityResponseException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}