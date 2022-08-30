using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Menu;

namespace XcellenceIT.Plugin.Misc.NewCRUD
{
    public class NewCRUDPlugin : BasePlugin, IAdminMenuPlugin
    {
        private readonly ILocalizationService _localizationServices;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;

        public NewCRUDPlugin(ILocalizationService localizationServices, IPermissionService permissionService, ISettingService settingService)
        {
            _localizationServices = localizationServices;
            _permissionService = permissionService;
            _settingService = settingService;
        }

        public async Task<bool> authenticate()
        {
            bool flag = false;
            if (await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }
        public override async Task InstallAsync()
        {
            await _localizationServices.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Misc.NewCRUD.Title"] = "XcellenceIT",
                ["Plugins.Misc.NewCRUD.MainMenu"] = "CRUD",
                ["Plugins.Misc.NewCRUD.MainMenu2"] = "CRUD Opration",
                ["Plugins.Misc.NewCRUD.Configure"] = "Setting",
                ["Plugins.Misc.NewCRUD.List"] = "Users List",
                ["Plugins.Misc.NewCRUD.Add.User"] = "Add New User",
                ["Plugins.Misc.NewCRUD.NewCRUD"] = "NewCRUD",
                ["Plugins.Misc.NewCRUD.BackToUserList"] = "Back To User List",
                ["Plugins.Misc.NewCRUD.User.Info"] = "User Info",
                ["Plugins.Misc.NewCRUD.Edit.User"] = "Edit User Details",

                ["Plugins.Misc.NewCRUD.Id"] = "Id",
                ["Plugins.Misc.NewCRUD.FirstName"] = "FirstName",
                ["Plugins.Misc.NewCRUD.LastName"] = "LastName",
                ["Plugins.Misc.NewCRUD.Age"] = "Age",
                ["Plugins.Misc.NewCRUD.City"] = "City",


            });
            await base.InstallAsync();
        }
        public override async Task UninstallAsync()
        {
            await _settingService.DeleteSettingAsync<NewCRUDSetting>();

            await _localizationServices.DeleteLocaleResourcesAsync("Plugins.Misc.NewCRUD");

            await base.UninstallAsync();
        }

        [Area(AreaNames.Admin)]
        public async Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
            var mainMenuItem = new SiteMapNode()
            {
                Title = await _localizationServices.GetResourceAsync("Plugins.Misc.NewCRUD.Title"),
                Visible = await authenticate(),
                IconClass = "fab fa-buysellads"
            };

            var pluginMenuItem = new SiteMapNode()
            {
                Title = await _localizationServices.GetResourceAsync("Plugins.Misc.NewCRUD.MainMenu"),
                Visible = await authenticate(),
                IconClass = "far fa-dot-circle"
            };

            mainMenuItem.ChildNodes.Add(pluginMenuItem);

            var configure = new SiteMapNode()
            {
                SystemName = "Plugins.Misc.NewCRUD.NewCRUD",
                Title = await _localizationServices.GetResourceAsync("Plugins.Misc.NewCRUD.Configure"),
                ControllerName = "NewCRUD",
                ActionName = "Configure",
                Visible = await authenticate(),
                IconClass = "far fa-circle",
                RouteValues = new RouteValueDictionary() { { "XcellenceIT.Plugin.Misc.NewCRUD", null } },
            };
            pluginMenuItem.ChildNodes.Add(configure);

            var list = new SiteMapNode()
            {
                SystemName = "Plugins.Misc.NewCRUD.MainMenu2",
                Title = await _localizationServices.GetResourceAsync("Plugins.Misc.NewCRUD.MainMenu2"),
                ControllerName = "NewCRUD",
                ActionName = "List",
                Visible = await authenticate(),
                IconClass = "far fa-circle",
                RouteValues = new RouteValueDictionary() { { "area", AreaNames.Admin } },
            };

            pluginMenuItem.ChildNodes.Add(list);

            var newCrudSettings = await _settingService.LoadSettingAsync<NewCRUDSetting>();

            var title = await _localizationServices.GetResourceAsync("Plugins.Misc.NewCRUD.Title");
            var targetMenu = rootNode.ChildNodes.FirstOrDefault(x => x.Title == title);

            if (targetMenu != null)
                targetMenu.ChildNodes.Add(pluginMenuItem);
            else
                rootNode.ChildNodes.Add(mainMenuItem);

        }
    }
}
