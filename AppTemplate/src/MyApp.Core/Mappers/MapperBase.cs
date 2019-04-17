using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Core.Mappers
{
    public abstract class MapperBase<TModel, TView>
        where TModel : new()
    {
        public TModel ToDataModel(TView view)
        {
            var model = new TModel();
            ToDataModel(model, view); //pass by ref
            return model;
        }

        protected abstract void ToDataModel(TModel model, TView view);

        public abstract TView ToViewModel(TModel model);
    }
}
