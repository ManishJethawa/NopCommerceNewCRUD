using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace XcellenceIT.Plugin.Misc.NewCRUD.Models
{
    public record ConfigurationModel : BaseNopModel
    {
        public bool Enable { get; set; }
    }
}
