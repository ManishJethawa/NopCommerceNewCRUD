using Nop.Web.Framework.Models;

namespace XcellenceIT.Plugin.Misc.NewCRUD.Models
{
    public record ConfigurationModel : BaseNopModel
    {
        public bool Enable { get; set; }
    }
}
