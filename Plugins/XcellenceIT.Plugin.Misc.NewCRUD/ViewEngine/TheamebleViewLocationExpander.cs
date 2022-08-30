using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor;
using Nop.Core.Infrastructure;
using Nop.Services.Plugins;
using Nop.Web.Framework;
using Nop.Web.Framework.Themes;

namespace XcellenceIT.Plugin.Misc.NewCRUD.ViewEngine
{
    public class TheamebleViewLocationExpander : IViewLocationExpander
    {
        private const string THEME_KEY = "nop.themename";
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            context.Values.TryGetValue(THEME_KEY, out string theme);
            {
                if (context.AreaName == AreaNames.Admin && context.ControllerName.Equals("NewCRUD")
                    && context.ViewName.Equals("Configure"))
                        {
                            viewLocations = new[]
                            {
                              $"~/Plugins/Misc.MyCustomPlugin/Views/Configure.cshtml"
                            }.Concat(viewLocations);
                
                        }
                if (context.AreaName == AreaNames.Admin && context.ControllerName.Equals("NewCRUD")
                    && context.ViewName.Equals("Edit"))
                        {
                            viewLocations = new[]
                            {
                              $"~/Plugins/Misc.MyCustomPlugin/Views/Edit.cshtml"
                            }.Concat(viewLocations);
                
                        }
            }

            return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            if (context.AreaName?.Equals(AreaNames.Admin) ?? false)
                return;

            context.Values[THEME_KEY] = EngineContext.Current.Resolve<IThemeContext>().GetWorkingThemeNameAsync().Result;
        }
    }
}
