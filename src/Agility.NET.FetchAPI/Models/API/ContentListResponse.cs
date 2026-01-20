using System.Collections.Generic;


namespace Agility.NET.FetchAPI.Models.API
{
    public class ContentListResponse<T>
    {
        public List<GenericContentItem<T>> Items { get; set; }
        public int TotalCount { get; set; }

    }
}
