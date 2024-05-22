using System;

namespace Agility.NET.FetchAPI.Models
{
    public class Properties
    {
        public int State { get; set; }
        public DateTime Modified { get; set; }
        public int VersionID { get; set; }
        public string ReferenceName { get; set; }
        public string DefinitionName { get; set; }
        public int ItemOrder { get; set; }
    }
}
