using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace DLeh.Util.Json
{
    public static class JsonUtil
    {
        public static string Serialize(object obj) => JsonConvert.SerializeObject(obj);
        public static T Deserialize<T>(string json)
        {
            if (json == null)
                return default(T);
            return JsonConvert.DeserializeObject<T>(json);
        }

        //interface members only
        public static JsonSerializerSettings GetInterfaceContractResolverSettings<TInterface>()
                where TInterface : class
            => new JsonSerializerSettings { ContractResolver = new InterfaceContractResolver<TInterface>() };

        public static TConcrete DeserializeInterface<TConcrete, TInterface>(string json)
                where TInterface : class
            => JsonConvert.DeserializeObject<TConcrete>(json, GetInterfaceContractResolverSettings<TInterface>());

        public static string SerializeInterface<TInterface>(object obj)
                where TInterface : class
            => JsonConvert.SerializeObject(obj, GetInterfaceContractResolverSettings<TInterface>());


        //child only
        public static JsonSerializerSettings GetChildContractResolverSettings<TInterface>()
                where TInterface : class
            => new JsonSerializerSettings { ContractResolver = new ChildContractResolver<TInterface>() };

        public static string SerializeChild<TInterface>(object obj)
                where TInterface : class
            => JsonConvert.SerializeObject(obj, GetChildContractResolverSettings<TInterface>());

        public static TConcrete DeserializeChild<TConcrete, TInterface>(string json)
                where TInterface : class
            => JsonConvert.DeserializeObject<TConcrete>(json, GetChildContractResolverSettings<TInterface>());


        public static T DeserializeWithPrivateSetters<T>(this string json)
        {
            var settings = new JsonSerializerSettings { ContractResolver = new PrivateSetterContractResolver() };
            return JsonConvert.DeserializeObject<T>(json, settings);
        }
    }
}
