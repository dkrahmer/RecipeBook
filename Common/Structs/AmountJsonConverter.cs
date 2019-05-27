using Newtonsoft.Json;
using System;

namespace Common.Structs
{
	internal class AmountJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return true;
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			// we currently support only writing of JSON
			return null;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null || !(value is Amount))
			{
				serializer.Serialize(writer, null);
				return;
			}

			var amount = (Amount)value;
			serializer.Serialize(writer, amount.ToString());
		}
	}
}