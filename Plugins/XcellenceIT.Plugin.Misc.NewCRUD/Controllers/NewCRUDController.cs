using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using XcellenceIT.Plugin.Misc.NewCRUD.Domain;
using XcellenceIT.Plugin.Misc.NewCRUD.Factories;
using XcellenceIT.Plugin.Misc.NewCRUD.ImportExport;
using XcellenceIT.Plugin.Misc.NewCRUD.Models;
using XcellenceIT.Plugin.Misc.NewCRUD.Services;

namespace XcellenceIT.Plugin.Misc.NewCRUD.Controllers
{
    [Area(AreaNames.Admin)]
    [AuthorizeAdmin]
    [AutoValidateAntiforgeryToken]
    public class NewCRUDController : BasePluginController
    {
        #region Fields
        private readonly ILocalizationService _localizationServices;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly INewCRUDExportManager _newCRUDExportManager;
        private readonly INewCRUDImportManager _newCRUDImportManager;
        private readonly INewCRUDModelFactories _newCRUDModelFactories;
        private readonly INewCRUDModelServices _newCRUDModelServices;
        #endregion

        #region Constructor
        public NewCRUDController(ILocalizationService localizationServices,
            IPermissionService permissionService,
            ISettingService settingService,
            INewCRUDExportManager newCRUDExportManager,
            INewCRUDImportManager newCRUDImportManager,
            INewCRUDModelFactories newCRUDModelFactories,
            INewCRUDModelServices newCRUDModelServices)
        {
            _localizationServices = localizationServices;
            _permissionService = permissionService;
            _settingService = settingService;
            _newCRUDExportManager = newCRUDExportManager;
            _newCRUDImportManager = newCRUDImportManager;
            _newCRUDModelFactories = newCRUDModelFactories;
            _newCRUDModelServices = newCRUDModelServices;
        }
        #endregion

        #region Configure Method
        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            var newCrudSetings = await _settingService.LoadSettingAsync<NewCRUDSetting>();
            var model = new ConfigurationModel
            {
                Enable = newCrudSetings.Enable
            };

            return View("~/Plugins/Misc.NewCRUD/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            var newCrudSettings = await _settingService.LoadSettingAsync<NewCRUDSetting>();
            newCrudSettings.Enable = model.Enable;

            await _settingService.SaveSettingOverridablePerStoreAsync(newCrudSettings, x => x.Enable, model.Enable);

            await _settingService.ClearCacheAsync();

            return await Configure();

        }
        #endregion

        #region List Method
        public virtual async Task<IActionResult> List()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();


            var model = await _newCRUDModelFactories.PreapareNewCRUDSearchModelAsync(new NewCRUDSearchModel());

            return View("~/Plugins/Misc.NewCRUD/Views/List.cshtml", model);
        }
        [HttpPost]
        public virtual async Task<IActionResult> UserList(NewCRUDSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            var model = await _newCRUDModelFactories.PreapareNewCRUDListModelAsync(searchModel);
            return Json(model);
        }

        #endregion

        #region Create,Edit,Delete
        public virtual async Task<IActionResult> Create()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            var model = await _newCRUDModelFactories.PrepareNewCRUDModelAsync(new NewCRUDModel(), new NewCRUDDomain());
            return View("~/Plugins/Misc.NewCRUD/Views/Create.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> Create(NewCRUDModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var user = new NewCRUDDomain()
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Age = model.Age,
                    City = model.City
                };
                await _newCRUDModelServices.InsertUser(user);

                if (!continueEditing)
                    return RedirectToAction("List");
                return RedirectToAction("Edit", new { id = user.Id });

            }
            return View(model);
        }
        public virtual async Task<IActionResult> Edit(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            var user = await _newCRUDModelServices.GetUserByIdAsync(id);
            if (user == null)
                RedirectToAction("List");

            var model = await _newCRUDModelFactories.PrepareNewCRUDModelAsync(null, user);
            return View("~/Plugins/Misc.NewCRUD/Views/Edit.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> Edit(NewCRUDModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            var user = await _newCRUDModelServices.GetUserByIdAsync(model.Id);

            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    user.Id = model.Id;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Age = model.Age;
                    user.City = model.City;
                }

                await _newCRUDModelServices.UpdateUsersAsync(user);

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = model.Id });
            }
            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Delete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            var user = await _newCRUDModelServices.GetUserByIdAsync(id);
            if (user == null)
                return RedirectToAction("List");

            await _newCRUDModelServices.DeleteUsersAsync(user);

            return RedirectToAction("List");
        }
        #endregion

        #region Import,Export
        public virtual async Task<IActionResult> ExportToXlsx()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();
            try
            {
                var bytes = await _newCRUDExportManager.ExportNewCRUDToXlsxAsync
                    ((await _newCRUDModelServices.GetAllUsersAsync()).ToList());
                return File(bytes, MimeTypes.TextXlsx, "Users.Xlsx");
            }
            catch
            {
                return RedirectToAction("List");
            }

        }
        [HttpPost]
        public virtual async Task<IActionResult> ImportFromXlsx(IFormFile importexcelfile)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCategories))
                return AccessDeniedView();


            if (importexcelfile != null && importexcelfile.Length > 0)
            {
                await _newCRUDImportManager.ImportNewCRUDFromXlsxAsync(importexcelfile.OpenReadStream());
            }
            else
            {
                return RedirectToAction("List");
            }

            return RedirectToAction("List");

        }
        #endregion
    }
}