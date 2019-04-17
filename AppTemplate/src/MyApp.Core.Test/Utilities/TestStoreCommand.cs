using MyApp.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Core.Test
{
    public class TestStoreCommand<T> : IStoreCommand<T>
    {
        public StoreInteractionType Type { get; }
        public object Entity { get; }
        public T EntityTyped { get; }

        public TestStoreCommand(StoreInteractionType type, T entity)
        {
            Type = type;
            Entity = entity;
            EntityTyped = entity;
        }
    }
}
