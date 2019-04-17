using MyApp.Core.Configuration;
using MyApp.Core.Data;
using MyApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Data
{
    public class DataAccess : IDataAccess
    {
        public DataAccess(IDbConfiguration config)
        {
            Db = new Database(config);
        }

        public Database Db { get; }

        public IDataTransaction CreateTransaction()
        {
            return new DataTransaction(this);
        }

        public IEnumerable<T> Query<T>() where T : Entity
        {
            //hit the db
            return new List<T>();
        }
    }

    /// <summary>
    /// Represents the object that ACTUALLY connects to the database, such as an EF DbContext
    /// </summary>
    public class Database
    {
        private readonly IDbConfiguration _config;

        public Database(IDbConfiguration config)
        {
            _config = config;
        }

        private void Connect()
        {
            var connString = _config.ConnectionString;
            //use connString to connect to db
        }


        public void SaveChanges()
        {
            //impl


        }

    }
}
