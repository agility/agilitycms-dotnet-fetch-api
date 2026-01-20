namespace Agility.NET.FetchAPI.Models.Data
{
    public class GetPageParameters
    {
        public bool IsPreview { get; set; }
        public string Locale { get; set; }
        public int PageId { get; set; }
        public int ContentLinkDepth { get; set; }
        public bool ExpandAllContentLinks { get; set; }
    }
}
