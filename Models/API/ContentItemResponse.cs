using Agility.NET5.Shared.Models;

namespace Agility.NET5.FetchAPI.Models.API
{
    public class ContentItemResponse<T>
    {
        public int ContentID { get; set; }
        public Properties Properties { get; set; }
        public T Fields { get; set; }
        public string ResponseMessage { get; set; }
    }
}
