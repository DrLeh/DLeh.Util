using MyApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Core.Data
{
    public interface IDataTransaction
    {
        void Add<T>(T entity) where T : Entity;
        void Update<T>(T entity) where T : Entity;
        void Remove<T>(T entity) where T : Entity;
        void Commit();
        IReadOnlyList<IStoreCommand> Commands { get; }
    }

    public enum StoreInteractionType
    {
        Added,
        Updated,
        Removed,
    }

    public interface IStoreCommand
    {
        StoreInteractionType Type { get; }
        object Entity { get; }
    }

    public interface IStoreCommand<out T> : IStoreCommand
    {
        T EntityTyped { get; }
    }
}
