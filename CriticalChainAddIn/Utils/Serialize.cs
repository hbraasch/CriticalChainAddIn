using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CriticalChainAddIn.Utils
{
    public class Serialize
    {

        public static string SerializeObjectToStringJson<T>(T t) => JsonConvert.SerializeObject(t, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

        public static T DeserializeObjectFromStringJson<T>(string jsonString)
        {
            T obj = JsonConvert.DeserializeObject<T>(jsonString, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            return obj;
        }
    }
}
