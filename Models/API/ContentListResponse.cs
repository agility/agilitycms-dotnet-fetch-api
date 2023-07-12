using System.Collections.Generic;
using Agility.NET.Shared.Models;

namespace Agility.NET.FetchAPI.Models.API
{
    public class ContentListResponse<T>
    {
        public List<GenericContentItem<T>> Items { get; set; }
        public int TotalCount { get; set; }
        public string ResponseMessage { get; set; }
    }
}
