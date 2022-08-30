using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Nop.Core;
using Nop.Services.ExportImport.Help;
using XcellenceIT.Plugin.Misc.NewCRUD.Domain;
using XcellenceIT.Plugin.Misc.NewCRUD.Services;

namespace XcellenceIT.Plugin.Misc.NewCRUD.ImportExport
{
    public partial class NewCRUDImportManager : INewCRUDImportManager

    {
        #region Fields
        private readonly INewCRUDModelServices _newCRUDModelServices;
        #endregion

        #region Constructor
        public NewCRUDImportManager(INewCRUDModelServices newCRUDModelServices)
        {
            _newCRUDModelServices = newCRUDModelServices;
        }
        #endregion

        #region Methods
        protected virtual Task UpdateNewCRUDByXlsxAsync(NewCRUDDomain newCRUDDomain, PropertyManager<NewCRUDDomain> manager, Dictionary<string, ValueTask<NewCRUDDomain>> allUsers, bool isNew)
        {
            foreach (var property in manager.GetProperties)
            {
                switch (property.PropertyName)
                {
                    case "FirstName":
                        newCRUDDomain.FirstName = property.StringValue.Split(new[] { ">>" }, StringSplitOptions.RemoveEmptyEntries).Last().Trim();
                        break;

                    case "LastName":
                        newCRUDDomain.LastName = property.StringValue.Split(new[] { ">>" }, StringSplitOptions.RemoveEmptyEntries).Last().Trim();
                        break;
                }
            }

            return Task.CompletedTask;
        }
        protected virtual async Task<(NewCRUDDomain newCRUDDomain, bool isNew)> GetNewCRUDFromXlsxAsync(PropertyManager<NewCRUDDomain> manager, IXLWorksheet worksheet, int iRow, Dictionary<string, ValueTask<NewCRUDDomain>> allUsers)
        {
            manager.ReadFromXlsx(worksheet, iRow);
            var newCRUDDomain = await await allUsers.Values.FirstOrDefaultAwaitAsync(async c => (await c).Id == manager.GetProperty("Id")?.IntValue);

            if (newCRUDDomain == null)
            {
                var userName = manager.GetProperty("Name").StringValue;

                if (!string.IsNullOrEmpty(userName))
                {
                    newCRUDDomain = allUsers.ContainsKey(userName)
                        ? await allUsers[userName]
                        : await await allUsers.Values.FirstOrDefaultAwaitAsync(async c => (await c).FirstName.Equals(userName, StringComparison.InvariantCulture));
                }
            }
            var isNew = newCRUDDomain == null;

            newCRUDDomain ??= new NewCRUDDomain();

            return (newCRUDDomain, isNew);
        }
        protected virtual async Task SaveNewCRUDAsync(bool isNew, NewCRUDDomain newCRUDDomain, Dictionary<string, ValueTask<NewCRUDDomain>> allUsers)
        {
            if (isNew)
                await _newCRUDModelServices.InsertUser(newCRUDDomain);
            else
                await _newCRUDModelServices.UpdateUsersAsync(newCRUDDomain);
        }
        public virtual async Task ImportNewCRUDFromXlsxAsync(Stream stream) 
        {
            using var workboox = new XLWorkbook(stream);

            var worksheet = workboox.Worksheets.FirstOrDefault();
            if (worksheet == null)
                throw new NopException("No worksheet found");

            var properties = GetPropertiesByExcelCells<NewCRUDDomain>(worksheet);

            var manager = new PropertyManager<NewCRUDDomain>(properties, null);

            var iRow = 2;

            var allUsers = await _newCRUDModelServices.GetAllUserAsync();

            var saveNextTime = new List<int>();

            while (true)
            {
                var allColumnsAreEmpty = manager.GetProperties
                    .Select(property => worksheet.Row(iRow).Cell(property.PropertyOrderPosition))
                    .All(cell => string.IsNullOrEmpty(cell?.Value?.ToString()));

                if (allColumnsAreEmpty)
                    break;

                manager.ReadFromXlsx(worksheet, iRow);

                var newUser = await _newCRUDModelServices.GetUserByIdAsync(manager.GetProperty("Id").IntValue);

                var isNew = newUser == null;

                newUser ??= new NewCRUDDomain();

                foreach (var property in manager.GetProperties)
                {
                    switch (property.PropertyName)
                    {
                        case "FirstName":
                            newUser.FirstName = property.StringValue;
                            break;
                        case "LastName":
                            newUser.LastName = property.StringValue;
                            break;
                        case "Age":
                            newUser.Age = property.IntValue;
                            break;
                        case "City":
                            newUser.City = property.StringValue;
                            break;
                    }
                }

                if (isNew)
                    await _newCRUDModelServices.InsertUser(newUser);
                else
                    await _newCRUDModelServices.UpdateUsersAsync(newUser);

                iRow++;
            }
        }
        public static IList<PropertyByName<T>> GetPropertiesByExcelCells<T>(IXLWorksheet worksheet)
        {
            var properties = new List<PropertyByName<T>>();
            var poz = 1;
            while (true)
            {
                try
                {
                    var cell = worksheet.Row(1).Cell(poz);

                    if (string.IsNullOrEmpty(cell?.Value?.ToString()))
                        break;

                    poz += 1;
                    properties.Add(new PropertyByName<T>(cell.Value.ToString()));
                }
                catch
                {
                    break;
                }
            }

            return properties;
        }
        #endregion
    }
}
