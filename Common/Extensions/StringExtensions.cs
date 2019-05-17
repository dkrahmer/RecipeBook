using Newtonsoft.Json;

namespace Common.Extensions
{
    public static class StringExtensions
    {
        public static T JsonToObject<T>(this string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
