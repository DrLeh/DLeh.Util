using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace DLeh.Util.Json
{
    //http://stackoverflow.com/a/29440580/526704
    /// <summary>
    /// Will only serialize values that are from <see cref="TInterface"/>
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    public class InterfaceContractResolver<TInterface> : DefaultContractResolver
        where TInterface : class
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(typeof(TInterface), memberSerialization);
            return properties;
        }
    }



    /// <summary>
    /// Will only serialize values that aren't inherited from <see cref="TInterface"/>
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    public class ChildContractResolver<TInterface> : DefaultContractResolver
        where TInterface : class
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);
            IList<JsonProperty> interfaceProperties = base.CreateProperties(typeof(TInterface), memberSerialization);

            foreach (var property in properties)
                property.ShouldSerialize = x => !interfaceProperties.Any(y => y.PropertyName == property.PropertyName);

            return properties;
        }
    }
}
