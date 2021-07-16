using System.Collections.Generic;
using Agility.NET5.Shared.Models;

namespace Agility.NET5.FetchAPI.Models.API
{
    public class ContentListResponse<T>
    {
        public List<GenericContentItem<T>> Items { get; set; }
        public int TotalCount { get; set; }
}
}
