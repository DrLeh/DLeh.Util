using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyApp.Core.Configuration;
using MyApp.Core.Models;
using System;

namespace MyApp.Console
{
    public class Program
    {
        private static IServiceProvider container;

        static void Startup()
        {
            var services = new ServiceCollection();
            new Registry().RegisterServices(services);
            container = services
                .AddLogging()
                .BuildServiceProvider();

            //configure console logging
            container
                .GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug);

            var logger = container.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            logger.LogDebug("Starting application");
        }

        static void Main(string[] args)
        {
            Startup();


            //you can build this out to be an interactive console app to run,
            // or just comment out what code you don't want to run
            // but if you're trying to get something figured out, it's a lot easier to
            // just run this console app with everything configured how you expect it
            // than it is to fire up the web app, click through, enter fields, etc.
            ConfigurationTest();
            //DoSomethingElse();


            System.Console.WriteLine("Done! Press enter to exit");
            System.Console.ReadLine();
        }

        public static void ConfigurationTest()
        {
            var config = container.GetService<IConfiguration>();
            System.Console.WriteLine($"MySetting: {config.MySetting}");
            System.Console.WriteLine($"Db Setting: {config.Db.ConnectionString}");
        }
    }
}
