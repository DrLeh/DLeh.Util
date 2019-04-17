using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Configuration;
using MyApp.Core.Startup;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Core.Test.Startup
{
    //Even registrations can be unit tested, nothing is safe!
    [TestFixture]
    public class RegistryTest
    {
        [Test]
        public void Register_Test()
        {
            var services = new ServiceCollection();

            //act
            new Registry().RegisterServices(services);

            var container = services.BuildServiceProvider();
        }
    }
}
