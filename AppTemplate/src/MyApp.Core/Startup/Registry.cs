using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Configuration;
using MyApp.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Core.Startup
{
    public class Registry
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IConfiguration, FileConfiguration>();
            //register the dbconfiguration for when you only need the IDbConfiguration
            services.AddTransient(sp => sp.GetService<IConfiguration>().Db);

        }
    }
}
