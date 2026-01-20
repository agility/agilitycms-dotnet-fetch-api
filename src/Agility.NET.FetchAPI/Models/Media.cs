using System;

namespace Agility.NET.FetchAPI.Models
{
    public class Media
    {
        public int MediaID { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
        public int Size { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public MetaData MetaData { get; set; }

    }
}
