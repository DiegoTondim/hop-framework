using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Text;

namespace Hop.Framework.Core.Extensions
{
    public static class SerializerExtensions
    {
        public static T ParseTo<T>(this byte[] source)
        {
            var sourceMessage = Encoding.UTF8.GetString(source);
            var settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Objects;
            return JsonConvert.DeserializeObject<T>(sourceMessage, settings);
        }

        public static object ParseToType(this byte[] source, Type sourceType)
        {
            var sourceMessage = Encoding.UTF8.GetString(source);
            var settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Objects;
            return JsonConvert.DeserializeObject(sourceMessage, sourceType, settings);
        }

        public static byte[] Serialize(this object source)
        {
            var settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Objects;
            return JsonConvert.SerializeObject(source, settings).GetBytes();
        }

        public static string SerializeToJson(this object source)
        {
            var settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Objects;
            return JsonConvert.SerializeObject(source, settings);
        }

        public static T ParseFromJson<T>(this string source)
        {
            if (string.IsNullOrEmpty(source)) return default(T);

            var settings = new JsonSerializerSettings();
            var dcr = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            dcr.DefaultMembersSearchFlags |= BindingFlags.NonPublic;
            settings.ContractResolver = dcr;
            settings.TypeNameHandling = TypeNameHandling.All;

            return JsonConvert.DeserializeObject<T>(source, settings);
        }
    }
}