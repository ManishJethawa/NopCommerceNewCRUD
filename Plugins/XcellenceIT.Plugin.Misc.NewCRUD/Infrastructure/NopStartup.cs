using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using XcellenceIT.Plugin.Misc.NewCRUD.Factories;
using XcellenceIT.Plugin.Misc.NewCRUD.ImportExport;
using XcellenceIT.Plugin.Misc.NewCRUD.Services;

namespace XcellenceIT.Plugin.Misc.NewCRUD.Infrastructure
{
    public class NopStartup : INopStartup
    {
        public int Order => 0;

        public void Configure(IApplicationBuilder application)
        {
        }
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<INewCRUDModelFactories, NewCRUDModelFactories>();
            services.AddScoped<INewCRUDModelServices, NewCRUDModelServices>();
            services.AddScoped<INewCRUDExportManager, NewCRUDExportManager>();
            services.AddScoped<INewCRUDImportManager, NewCRUDImportManager>();
        }
    }
}
