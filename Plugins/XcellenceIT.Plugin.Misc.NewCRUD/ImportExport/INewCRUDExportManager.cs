using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XcellenceIT.Plugin.Misc.NewCRUD.Domain;

namespace XcellenceIT.Plugin.Misc.NewCRUD.ImportExport
{
    public partial interface INewCRUDExportManager
    {
        Task<byte[]> ExportNewCRUDToXlsxAsync(IList<NewCRUDDomain> newCRUDDomain);
    }
}
