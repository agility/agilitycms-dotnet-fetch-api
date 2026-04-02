using System;
using System.Collections.Generic;

namespace Agility.NET.FetchAPI.Models
{
    public class ContentDefinition
    {
        public int Id { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string DisplayName { get; set; }
        public string ReferenceName { get; set; }
        public List<string> ContentReferenceNames { get; set; }
        public List<Field> Fields { get; set; }

    }
}
