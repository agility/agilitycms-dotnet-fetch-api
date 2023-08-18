using Agility.NET.Shared.Models;

namespace Agility.NET.FetchAPI.Models.API
{
    public class ContentItemResponse<T>
    {
        public int ContentID { get; set; }
        public Properties? Properties { get; set; }
        public T Fields { get; set; }
        public string? ResponseMessage { get; set; }
    }
}
