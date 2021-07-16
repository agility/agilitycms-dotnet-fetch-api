namespace Agility.NET5.FetchAPI.Models.Data
{
    public class GetPageParameters
    {
        public string Locale { get; set; }
        public int PageId { get; set; }
        public int ContentLinkDepth { get; set; }
        public bool ExpandAllContentLinks { get; set; }
    }
}
