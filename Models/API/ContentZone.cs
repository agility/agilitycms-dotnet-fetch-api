using System.Collections.Generic;
using Agility.NET5.Shared.Models;

namespace Agility.NET5.FetchAPI.Models.API
{
    public class ContentZone
    {
        public string ReferenceName { get; set; }
        public List<Module_Model> Modules { get; set; }
    }
}
