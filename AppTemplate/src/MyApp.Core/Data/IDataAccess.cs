using MyApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Core.Data
{
    public interface IDataAccess
    {
        IDataTransaction CreateTransaction();
        IEnumerable<T> Query<T>() where T : Entity;
    }
}
