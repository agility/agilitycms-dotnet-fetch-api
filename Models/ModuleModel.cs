using Agility.NET5.FetchAPI.Models.Data;
using Agility.NET5.Shared.Models;
using Agility.NET5.FetchAPI.Models.API;

namespace Agility.NET5.FetchAPI.Models
{
    public class ModuleModel
    {
        public string Module { get; set; }
        public string Locale { get; set; }
        public Module_Model Model { get; set; }
        public SitemapPage SitemapPage { get; set; }
    }
}
