using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Core.Configuration
{
    public interface IConfiguration
    {
        string MySetting { get; }

        IDbConfiguration Db { get; }
    }

    public interface IDbConfiguration
    {
        string ConnectionString { get; }
    }
}
