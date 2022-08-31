using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace XcellenceIT.Plugin.Misc.NewCRUD.Infrastructure
{
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapControllerRoute(name: NewCRUDDefaults.ConfigurationRouteName,
                pattern: "Admin/NewCRUD/Configure",
                defaults: new { controller = "NewCRUD", action = "Configure" });
        }
        public int Priority => 0;
    }
}
