using Agility.NET.FetchAPI.Models.Data;
using Agility.NET.Shared.Models;
using Agility.NET.FetchAPI.Models.API;

namespace Agility.NET.FetchAPI.Models
{
    public class ModuleModel
    {
        public string Module { get; set; }
        public string Locale { get; set; }
        public Module_Model Model { get; set; }
        public SitemapPage SitemapPage { get; set; }
    }
}
