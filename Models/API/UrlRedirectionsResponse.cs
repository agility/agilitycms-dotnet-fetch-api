using System;
using System.Collections.Generic;
using Agility.NET5.Shared.Models;

namespace Agility.NET5.FetchAPI.Models.API
{
    public class UrlRedirectionsResponse
    {
        public List<UrlRedirection> Items { get; set; }
        public bool IsUpToDate { get; set; }
        public DateTime? LastAccessDate { get; set; }
    }
}
