using MyApp.Core.Data;
using MyApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyApp.Core.Test
{
    //todo: move this into another class that can be shared between other test projects
    public class TestDataTransaction : IDataTransaction
    {
        private List<IStoreCommand> _commands;

        public TestDataTransaction()
        {
            _commands = new List<IStoreCommand>();
        }

        public void Add<T>(T entity) where T : Entity
        {
            _commands.Add(new TestStoreCommand<T>(StoreInteractionType.Added, entity));
        }

        public void Update<T>(T entity) where T : Entity
        {
            _commands.Add(new TestStoreCommand<T>(StoreInteractionType.Updated, entity));
        }

        public void Remove<T>(T entity) where T : Entity
        {
            _commands.Add(new TestStoreCommand<T>(StoreInteractionType.Removed, entity));
        }


        public void Commit()
        {
        }

        public IReadOnlyList<IStoreCommand> Commands => _commands;

        public IEnumerable<IStoreCommand> Added => Commands.Where(x => x.Type == StoreInteractionType.Added);
        public IEnumerable<IStoreCommand> Updated => Commands.Where(x => x.Type == StoreInteractionType.Updated);
        public IEnumerable<IStoreCommand> Removed => Commands.Where(x => x.Type == StoreInteractionType.Removed);

        public IEnumerable<object> AddedEntity => Commands.Where(x => x.Type == StoreInteractionType.Added).Select(x => x.Entity);
        public IEnumerable<object> UpdatedEntity => Commands.Where(x => x.Type == StoreInteractionType.Updated).Select(x => x.Entity);
        public IEnumerable<object> RemovedEntity => Commands.Where(x => x.Type == StoreInteractionType.Removed).Select(x => x.Entity);

        public IEnumerable<T> AddedEntityOfType<T>() => Commands.Where(x => x.Type == StoreInteractionType.Added).Select(x => x.Entity).OfType<T>();
        public IEnumerable<T> UpdatedEntityOfType<T>() => Commands.Where(x => x.Type == StoreInteractionType.Updated).Select(x => x.Entity).OfType<T>();
        public IEnumerable<T> RemovedEntityOfType<T>() => Commands.Where(x => x.Type == StoreInteractionType.Removed).Select(x => x.Entity).OfType<T>();
    }
}

