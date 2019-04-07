using System;
using System.Text;
using Newtonsoft.Json;
using TypeStream.Abstractions;

namespace TypeStream.Json.Formatters
{
	public class JsonFormatter : IFormatter
	{
		public TValue Deserialize<TValue>(Type type, byte[] bytes)
		{
			var json = Encoding.UTF8.GetString(bytes);
			return (TValue)JsonConvert.DeserializeObject(json, type);
		}

		public byte[] Serialize<TValue>(TValue value)
		{
			var json = JsonConvert.SerializeObject(value);
			return Encoding.UTF8.GetBytes(json);
		}
	}
}
