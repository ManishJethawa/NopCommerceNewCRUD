using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;
using XcellenceIT.Plugin.Misc.NewCRUD.Factories;

namespace XcellenceIT.Plugin.Misc.NewCRUD.C
{

    public class NewCRUDNavigationViewComponent : NopViewComponent
    {
        private readonly INewCRUDModelFactories _newCRUDModelFactories;

        public NewCRUDNavigationViewComponent(INewCRUDModelFactories newCRUDModelFactories)
        {
            _newCRUDModelFactories = newCRUDModelFactories;
        }

        public async Task<IViewComponentResult> InvokeAsync(int selectedTabId = 0)
        {
            var model = await _newCRUDModelFactories.PrepareCustomerNavigationModelAsync(selectedTabId);
            return View(model);
        }
    }
}
