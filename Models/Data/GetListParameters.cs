namespace Agility.NET.FetchAPI.Models.Data
{
    public class GetListParameters
    {
        public string Locale { get; set; }
        public string ReferenceName { get; set; }
        public string Fields { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public string Filter { get; set; }
        public string Sort { get; set; }
        public string Direction { get; set; }
        public int ContentLinkDepth { get; set; }
        public bool ExpandAllContentLinks { get; set; }
    }
}
