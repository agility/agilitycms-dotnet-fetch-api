using System.Collections.Generic;
using Agility.NET5.Shared.Models;

namespace Agility.NET5.FetchAPI.Models.API
{
    public class GalleryResponse
    {
        public int GalleryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Media> Media { get; set; }
        public int Count { get; set; }
    }
}
