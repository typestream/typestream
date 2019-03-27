using System;
using System.IO;
using System.Threading.Tasks;
using TypeStream.Core;
using TypeStream.Extentions;

namespace TypeStream
{
	public class TypeStream
	{
		private readonly Stream input;
		private readonly Stream output;
		private readonly IFormatter formatter;
		private readonly IdCache idCache;

		public TypeStream(Stream input, Stream output, IFormatter formatter, IIdResolver idResolver)
		{
			this.input = input;
			this.output = output;
			this.formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
			this.idCache = new IdCache(idResolver ?? throw new ArgumentNullException(nameof(formatter)));
		}

		public void Register<T>()
		{
			this.idCache.Cache<T>();
		}

		public async Task Write<TValue>(TValue value)
		{
			var type = value.GetType();
			var typeId = this.idCache.GetId(type);
			var bytes = this.formatter.Serialize(value);

			await this.output.WriteByteArrayAsync(typeId);
			await this.output.WriteByteArrayAsync(bytes);
		}

		public async Task<TValue> Read<TValue>()
		{
			var typeId = await this.input.ReadByteArrayAsync();
			var type = this.idCache.GetTypeById(typeId);

			var bytes = await this.input.ReadByteArrayAsync();

			return this.formatter.Deserialize<TValue>(type, bytes);
		}

		public async Task FlushAsync()
		{
			await this.output.FlushAsync();
		}
	}
}
