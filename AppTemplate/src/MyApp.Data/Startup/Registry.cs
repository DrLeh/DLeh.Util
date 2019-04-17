using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Data.Startup
{
    public class Registry
    {
        public void RegisterServices(IServiceCollection services)
        {
            //here, we register using the real data access when running from the web app
            services.AddTransient<IDataAccess, DataAccess>();
        }
    }
}
