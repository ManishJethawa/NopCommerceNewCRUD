using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XcellenceIT.Plugin.Misc.NewCRUD.ImportExport
{
    public partial interface INewCRUDImportManager
    {
        Task ImportNewCRUDFromXlsxAsync(Stream stream);
    }
}
