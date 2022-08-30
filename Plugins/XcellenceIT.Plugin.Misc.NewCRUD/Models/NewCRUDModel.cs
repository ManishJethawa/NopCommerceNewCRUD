using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace XcellenceIT.Plugin.Misc.NewCRUD.Models
{
    public record NewCRUDModel : BaseNopEntityModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public string City  { get; set; }


    }
}
