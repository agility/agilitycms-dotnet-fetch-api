using System.Collections.Generic;
using Agility.NET.Shared.Models;

namespace Agility.NET.FetchAPI.Models.API
{
    public class ContentZone
    {
        public string ReferenceName { get; set; }
        public List<Module_Model> Modules { get; set; }
    }
}
