using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Services.ExportImport.Help;
using XcellenceIT.Plugin.Misc.NewCRUD.Domain;
using XcellenceIT.Plugin.Misc.NewCRUD.Services;

namespace XcellenceIT.Plugin.Misc.NewCRUD.ImportExport
{
    public class NewCRUDExportManager : INewCRUDExportManager
    {
        private readonly INewCRUDModelServices _newCRUDModelServices;
        public NewCRUDExportManager(INewCRUDModelServices newCRUDModelServices)
        {
            _newCRUDModelServices = newCRUDModelServices;
        }
        public virtual async Task<byte[]> ExportNewCRUDToXlsxAsync(IList<NewCRUDDomain> newCRUDDomain)
        {
            var newCRUDParent = new List<NewCRUDDomain>();

            newCRUDParent.AddRange(await _newCRUDModelServices.GetUsersByIdsAsync(newCRUDDomain.Select(c => c.Id).Where(id => id != 0).ToArray()));

            var newCRUD = new PropertyManager<NewCRUDDomain>(new[]
            {
                new PropertyByName<NewCRUDDomain>("Id", m => m.Id ),
                new PropertyByName<NewCRUDDomain>("FirstName", m => m.FirstName),
                new PropertyByName<NewCRUDDomain>("LastName", m => m.LastName),
                new PropertyByName<NewCRUDDomain>("Age", m => m.Age),
                new PropertyByName<NewCRUDDomain>("City", m => m.City)

            }, null);

            return await newCRUD.ExportToXlsxAsync(newCRUDDomain);
        }
    }
}
