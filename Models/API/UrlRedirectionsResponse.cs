using System;
using System.Collections.Generic;
using Agility.NET.Shared.Models;

namespace Agility.NET.FetchAPI.Models.API
{
    public class UrlRedirectionsResponse
    {
        public List<UrlRedirection> Items { get; set; }
        public bool IsUpToDate { get; set; }
        public DateTime? LastAccessDate { get; set; }
    }
}
