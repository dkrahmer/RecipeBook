using Newtonsoft.Json;

namespace Common.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T JsonDeserialize<T>(this string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}