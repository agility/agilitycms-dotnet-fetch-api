﻿namespace Agility.NET.FetchAPI.Models.Data
{
    public class GetItemParameters
    {
        public string Locale { get; set; }
        public int ContentId { get; set; }
        public int ContentLinkDepth { get; set; }
        public bool ExpandAllContentLinks { get; set; }
    }
}
