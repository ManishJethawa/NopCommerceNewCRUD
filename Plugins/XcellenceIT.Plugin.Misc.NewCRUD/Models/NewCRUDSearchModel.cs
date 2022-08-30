using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace XcellenceIT.Plugin.Misc.NewCRUD.Models
{
    public partial record NewCRUDSearchModel : BaseSearchModel
    {
        public string SearchByFirstName { get; set; }

        public string SearchByLastName { get; set; }

        public int SearchByAge { get; set; }

        public string SearchByCity { get; set; }
    }
}
