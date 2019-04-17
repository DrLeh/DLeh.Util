using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Data;
using MyApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Web
{
    public class Registry
    {
        public void RegisterServices(IServiceCollection services)
        {
            //by abstracting these into a Registry, if you have a second app you want to run with
            // the same codebase, you can just call these registries. Some of them might have
            // access to different things so this list could vary by app.
            new MyApp.Core.Startup.Registry().RegisterServices(services);
            new MyApp.Data.Startup.Registry().RegisterServices(services);
        }
    }
}
