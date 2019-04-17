using MyApp.Core.Data;
using MyApp.Core.Models;
using System;
using System.Collections.Generic;

namespace MyApp.Data
{
    //here you'd implement this interface, and configure how those commands get executed
    // be it via Entity Framework, or what have you
    public class MyCommand<TModel> : IStoreCommand
        where TModel: Entity
    {
        public StoreInteractionType Type { get; }

        public object Entity { get; }

        public MyCommand(StoreInteractionType action, Entity model)
        {
            Type = action;
            Entity = model;
        }
    }

    public class DataTransaction : IDataTransaction
    {
        public IReadOnlyList<IStoreCommand> Commands => _commands;
        private readonly DataAccess _dataAccess;
        private readonly List<IStoreCommand> _commands;
        private readonly List<Action> _beforeCommit;
        private readonly List<Action> _afterCommit;

        public DataTransaction(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            _commands = new List<IStoreCommand>();
            _beforeCommit = new List<Action>();
            _afterCommit = new List<Action>();
        }

        public void Add<T>(T entity)
            where T : Entity
        {
            _commands.Add(new MyCommand<T>(StoreInteractionType.Added, entity));
        }

        public void Update<T>(T entity)
            where T : Entity
        {
            _commands.Add(new MyCommand<T>(StoreInteractionType.Updated, entity));
        }

        public void Remove<T>(T entity)
            where T : Entity
        {
            _commands.Add(new MyCommand<T>(StoreInteractionType.Removed, entity));
        }

        public void Commit()
        {
            //actually go to the database, make the changes recorded in Commands
            var db = _dataAccess.Db;

            //db.DoAdds();
            //db.DoUpdates();
            //db.DoRemoval();

            db.SaveChanges();
        }
    }
}
