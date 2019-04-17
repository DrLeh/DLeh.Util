using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyApp.Core.Configuration
{
    public class FileConfiguration : IConfiguration
    {
        protected IConfigurationRoot config;

        public FileConfiguration()
        {
            config = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .Build();
        }

        public string MySetting => config[nameof(MySetting)];

        private IDbConfiguration _db;
        public IDbConfiguration Db => _db ?? (_db = new DbConfiguration(config));
    }

    public class DbConfiguration : IDbConfiguration
    {
        protected IConfigurationRoot config;
        public DbConfiguration(IConfigurationRoot c) { config = c; }

        public string ConnectionString => config["Db:ConnectionString"];
    }
}
